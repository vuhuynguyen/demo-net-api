using ApplicationCore.Domain.Entities.Supporting;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EntityConfigurations
{
    class ChangeLogConfiguration : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            // table
            builder.ToTable(nameof(ChangeLog), DatabaseContext.SCHEMA);
            builder.HasIndex(x => new { x.EntityType, x.EntityId, x.ChangedDate }).IncludeProperties(x => new { x.ChangedBy });

            // props
            builder.Property(x => x.EntityId).IsRequired();
            builder.Property(x => x.EntityType).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.ChangedBy).IsRequired();
            builder.Property(x => x.ChangedDate).IsRequired();
        }
    }
}
