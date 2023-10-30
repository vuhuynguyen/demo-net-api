using ApplicationCore.Domain.SeedWork;

namespace ApplicationCore.Domain.Entities.Room;

public class RoomPricePeriod : Entity, IConcurrencyCheck, IAuditable, ISoftDeletable
{
    public decimal Price { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public RoomAvailability RoomAvailability { get; set; }
    
    public Guid RoomAvailabilityId { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string CreationBy { get; set; }
    
    public DateTime? ModificationDate { get; set; }
    
    public string ModificationBy { get; set; }

    public RoomPricePeriod()
    {
    }
}