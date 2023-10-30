using ApplicationCore.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Configures Primary Key, Soft Delete, Concurrency, Audit by convention
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureByConvention<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            builder.ConfigureEntityDefault();
            builder.ConfigureSoftDelete();
            builder.ConfigureConcurrencyCheck();
            builder.ConfigureAudit();
        }

        private static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(builder.Metadata.ClrType))
            {
                builder.Property<bool>("IsDeleted");
                builder.Property<string>("DeletionBy");
                builder.Property<DateTime?>("DeletionDate");

                builder.HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
            }
        }

        private static void ConfigureConcurrencyCheck<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            if (typeof(IConcurrencyCheck).IsAssignableFrom(builder.Metadata.ClrType))
            {
                builder.Property(nameof(IConcurrencyCheck.RowVersion))
                    .IsRowVersion();
            }
        }

        private static void ConfigureAudit<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            if (typeof(IAuditable).IsAssignableFrom(builder.Metadata.ClrType))
            {
                builder.Property(nameof(IAuditable.CreationBy)).IsRequired();
                builder.Property(nameof(IAuditable.CreationDate)).IsRequired();
            }
        }

        private static void ConfigureEntityDefault<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            if (typeof(Entity).IsAssignableFrom(builder.Metadata.ClrType))
            {
                builder.HasKey(nameof(Entity.Id)).IsClustered(false);
                builder.HasAlternateKey(nameof(Entity.IdentityKey)).IsClustered();
                builder.Property(nameof(Entity.IdentityKey)).UseIdentityColumn().ValueGeneratedOnAdd();
                builder.Ignore(nameof(Entity.DomainEvents));
                builder.Ignore(nameof(Entity.IsTransient));
            }
        }
    }
}
