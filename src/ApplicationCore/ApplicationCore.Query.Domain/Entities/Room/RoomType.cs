using ApplicationCore.Domain.SeedWork;

namespace ApplicationCore.Domain.Entities.Room;

public class RoomType : SimpleEntity, ISoftDeletable
{
    public string Name { get; set; }
    
    public int Capacity { get; set; }
    
    public ICollection<Room> Rooms { get; set; }
    
    public RoomAvailability RoomAvailability { get; set; }

    public RoomType(string name, int  capacity)
    {
        Name = name;
        Capacity = capacity;
    }
}