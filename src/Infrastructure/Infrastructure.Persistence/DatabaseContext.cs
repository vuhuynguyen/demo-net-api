using ApplicationCore.Common.Providers;
using ApplicationCore.Common.Shared;
using ApplicationCore.Domain.Entities.Supporting;
using ApplicationCore.Domain.SeedWork;
using ApplicationCore.Domain.Shared.Exceptions;
using EnsureThat;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public const string SCHEMA = "Application";

        private readonly ApplicationContext _applicationContext;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMediator _mediator;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DatabaseContext(ApplicationContext applicationContext,
            IConnectionStringProvider connectionStringProvider,
            IMediator mediator)
        {
            _applicationContext = applicationContext;
            _connectionStringProvider = connectionStringProvider;
            _mediator = mediator;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && _connectionStringProvider != null)
            {
                var connectionString = _connectionStringProvider.Get();
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", SCHEMA);
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureEntities(builder);
        }

        public void Migrate()
        {
            Database.Migrate();
        }

        public void MarkAsChanged<T>(T entity) where T : Entity
        {
            Entry<T>(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default, ConcurrencyResolutionStrategy strategy = ConcurrencyResolutionStrategy.None)
        {
            if (_mediator != null)
            {
                await _mediator.DispatchDomainEventsAsync(this);
            }

            bool saveFailed;

            switch (strategy)
            {
                case ConcurrencyResolutionStrategy.None:
                    try
                    {
                        PreSaveChanges();
                        await base.SaveChangesAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        var entry = ex.Entries.First();
                        var modificationBy = string.Empty;
                        entry.OriginalValues?.TryGetValue<string>("ModificationBy", out modificationBy);
                        throw new DbConcurrencyException(modificationBy);
                    }
                    break;

                case ConcurrencyResolutionStrategy.DatabaseWin:
                    do
                    {
                        saveFailed = false;

                        try
                        {
                            PreSaveChanges();
                            await base.SaveChangesAsync(cancellationToken);
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update the values of the Entity that failed to save from the store
                            ex.Entries.Single().Reload();
                        }
                    } while (saveFailed);

                    break;

                case ConcurrencyResolutionStrategy.ClientWin:
                    do
                    {
                        saveFailed = false;
                        try
                        {
                            PreSaveChanges();

                            await base.SaveChangesAsync(cancellationToken);
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            saveFailed = true;

                            // Update original values from the database
                            var entry = ex.Entries.Single();
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        }
                    } while (saveFailed);

                    break;
            }
        }

        private void ConfigureEntities(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void PreSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.Entity is Entity entity)
                {
                    if (entity.IsTransient)
                    {
                        entry.State = EntityState.Added;
                    }
                }

                if (entry.State == EntityState.Modified && entry.Entity is IConcurrencyCheck)
                {
                    HandleConcurrencyCheck(entry);
                }

                if ((entry.State == EntityState.Added || entry.State == EntityState.Modified) && (entry.Entity is IAuditable || entry.Entity is ICreationAuditable))
                {
                    HandleAudit(entry);
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable)
                {
                    HandleSoftDelete(entry);
                }
            }
        }

        private void HandleConcurrencyCheck(EntityEntry entry)
        {
            var entity = (IConcurrencyCheck)entry.Entity;
            entry.Property(nameof(IConcurrencyCheck.RowVersion)).OriginalValue = entity.RowVersion;
        }

        private void HandleSoftDelete(EntityEntry entry)
        {
            Ensure.That(_applicationContext.Principal, optsFn: o => o.WithMessage("Auditing failed, ApplicationContext Principal is null")).IsNotNull();

            entry.Property("IsDeleted").CurrentValue = true;
            entry.Property("DeletedBy").CurrentValue = _applicationContext.Principal.UserId;
            entry.Property("DeletionDate").CurrentValue = DateTime.UtcNow;
            entry.State = EntityState.Modified;
        }

        private void HandleAudit(EntityEntry entry)
        {
            Ensure.That(_applicationContext.Principal, optsFn: o => o.WithMessage("Auditing failed, ApplicationContext Principal is null")).IsNotNull();

            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreationBy = _applicationContext.Principal.UserId;
                    auditable.CreationDate = DateTime.UtcNow;
                }
                else
                {
                    auditable.ModificationBy = _applicationContext.Principal.UserId;
                    auditable.ModificationDate = DateTime.UtcNow;
                }
            }
            else if (entry.Entity is ICreationAuditable creationAuditable)
            {
                if (entry.State == EntityState.Added)
                {
                    creationAuditable.CreationBy = _applicationContext.Principal.UserId;
                    creationAuditable.CreationDate = DateTime.UtcNow;
                }
            }

            RecordChangeLog(entry);
        }

        private void RecordChangeLog(EntityEntry entry)
        {
            // We don't use composite key so it' safe to get first here
            var entityId = entry.Metadata.FindPrimaryKey().Properties.Select(p => entry.Property(p.Name).CurrentValue).First().ToString();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
            };

            var log = new ChangeLog
            {
                ChangedBy = _applicationContext.Principal.UserId,
                ChangedDate = DateTime.UtcNow,
                Data = JsonSerializer.Serialize(entry.Entity, options),
                EntityId = entityId ?? "",
                EntityType = entry.Metadata.ClrType.Name
            };

            Add(log);
        }
    }
}
