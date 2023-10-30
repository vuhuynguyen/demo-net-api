using ApplicationCore.Domain.Events;
using MediatR;


namespace ApplicationCore.Command.Features.Rooms.EventHandlers
{
    public class AddRoomEventHandlers : INotificationHandler<RoomAddedEvent>
    {
        
        public AddRoomEventHandlers()
        {
          
        }

        public async Task Handle(RoomAddedEvent notification, CancellationToken cancellationToken)
        {
        }
    }
}
