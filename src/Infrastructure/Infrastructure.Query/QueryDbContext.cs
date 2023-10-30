using ApplicationCore.Common.Providers;
using ApplicationCore.Query.Interfaces;
using ApplicationCoreQuery.DataModel.Seedwork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using ApplicationCoreQuery.DataModel.Rooms;

namespace Infrastructure.Query
{
    public class QueryDbContext : DbContext, IQueryDbContext
    {
        public const string SCHEMA = "Application";

        private readonly IConnectionStringProvider _connectionStringProvider;
        public bool IncludeDeleted { get; set; }

        public QueryDbContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && _connectionStringProvider != null)
            {
                var connectionString = _connectionStringProvider.Get();
                optionsBuilder.UseSqlServer(connectionString,
                    sqlOptions => { });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomAvailability>().ToTable(nameof(RoomAvailability), SCHEMA);
            modelBuilder.Entity<RoomType>().ToTable(nameof(RoomType), SCHEMA);
            modelBuilder.Entity<RoomPricePeriod>().ToTable(nameof(RoomPricePeriod), SCHEMA);

            // Global
            ConfigureGlobal(modelBuilder);
        }

        private void ConfigureGlobal(ModelBuilder modelBuilder)
        {
            var types = modelBuilder.Model.GetEntityTypes();
            foreach (var type in types)
            {
                var configureMethodInfo = typeof(QueryDbContext).GetMethod(nameof(ConfigureQueryFilter), BindingFlags.Instance | BindingFlags.NonPublic);
                configureMethodInfo
                    .MakeGenericMethod(type.ClrType)
                    .Invoke(this, new object[] { modelBuilder });
            }
        }

        private void ConfigureQueryFilter<T>(ModelBuilder builder) where T : class
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(T)))
            {
                Expression<Func<T, bool>> softDeleteFilter = e => IncludeDeleted || !((ISoftDeletable)e).IsDeleted;
                builder.Entity<T>().HasQueryFilter(softDeleteFilter);
            }
        }
    }
}
