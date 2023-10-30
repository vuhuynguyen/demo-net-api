using ApplicationCore.Domain.Entities.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        // table
        builder.ToTable(nameof(RoomType), DatabaseContext.SCHEMA);
        builder.ConfigureByConvention();

        // props
        builder.Property(x => x.Name)
            .IsRequired()
            .IsUnicode();

        builder.HasIndex(e => e.Name)
            .IsUnique();
        
        builder.Property(x => x.Capacity).IsRequired();
        
        // FK
        builder.HasOne<RoomAvailability>(rt => rt.RoomAvailability)
            .WithOne(r => r.Type)
            .HasForeignKey<RoomAvailability>(e => e.RoomTypeId);
    }
}