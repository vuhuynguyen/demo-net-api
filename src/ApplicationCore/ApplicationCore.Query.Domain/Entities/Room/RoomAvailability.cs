using ApplicationCore.Domain.SeedWork;

namespace ApplicationCore.Domain.Entities.Room;

public class RoomAvailability : Entity, IConcurrencyCheck, IAuditable, ISoftDeletable
{
    public RoomType Type { get; set; }
    
    public int RoomTypeId { get; set; }
    
    public int TotalRooms { get; set; }
    
    public int AvailableRooms { get; set; }
    
    public ICollection<RoomPricePeriod> PricePeriods { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string CreationBy { get; set; }
    
    public DateTime? ModificationDate { get; set; }
    
    public string ModificationBy { get; set; }

    public RoomAvailability()
    {
            
    }
    
    public decimal GetPrice()
    {
        // Find the corresponding RoomPricePeriod for the provided RoomAvailability
        var pricePeriod = PricePeriods.FirstOrDefault(period =>
            DateTime.Now >= period.StartDate && DateTime.Now <= period.EndDate);

        return pricePeriod?.Price ?? 0;
    }
}