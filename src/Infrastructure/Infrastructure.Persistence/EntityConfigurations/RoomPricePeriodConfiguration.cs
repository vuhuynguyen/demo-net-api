using ApplicationCore.Domain.Entities.Room;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomPricePeriodConfiguration : IEntityTypeConfiguration<RoomPricePeriod>
{
    public void Configure(EntityTypeBuilder<RoomPricePeriod> builder)
    {
        // table
        builder.ToTable(nameof(RoomPricePeriod), DatabaseContext.SCHEMA);
        builder.ConfigureByConvention();
        
        // FK
        builder.HasOne<RoomAvailability>(rpp => rpp.RoomAvailability)
            .WithMany(ra => ra.PricePeriods)
            .HasForeignKey(e => e.RoomAvailabilityId);
    }
}