using ApplicationCore.Domain.SeedWork;

namespace ApplicationCore.Domain.Entities.Room;

public class Room : Entity, IConcurrencyCheck, IAuditable, ISoftDeletable, IAggregateRoot
{
    public string Name { get; set; }
    
    public RoomType Type { get; set; }
    
    public int RoomTypeId { get; set; }
    
    public bool IsOccupied { get; set; }
    
    public byte[] RowVersion { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string CreationBy { get; set; }
    
    public DateTime? ModificationDate { get; set; }
    
    public string ModificationBy { get; set; }
}