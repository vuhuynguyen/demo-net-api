using ApplicationCore.Domain.Entities.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
{
    public void Configure(EntityTypeBuilder<RoomAvailability> builder)
    {
        // table
        builder.ToTable(nameof(RoomAvailability), DatabaseContext.SCHEMA);
        builder.ConfigureByConvention();

        // props
        builder.Property(x => x.RoomTypeId).IsRequired();
    }
}