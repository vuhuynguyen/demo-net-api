using ApplicationCore.Domain.Entities.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        // table
        builder.ToTable(nameof(Room), DatabaseContext.SCHEMA);
        builder.ConfigureByConvention();

        // props
        builder.Property(x => x.Name).IsRequired();
        
        // fk
        builder.HasOne<RoomType>(r => r.Type)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(e => e.RoomTypeId);
    }
}