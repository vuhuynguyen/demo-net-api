using ApplicationCore.Domain.Entities.Room;

namespace ApplicationCore.Domain.Events
{
    public class RoomAddedEvent : BaseDomainEvent
    {
        public Room Room { get; }

        public RoomAddedEvent(Room room, string source) : base(source)
        {
            Room = room;
        }
    }
}
