using ApplicationCore.Domain.Entities.Room;
using ApplicationCore.Domain.Repositories.Rooms;

namespace Infrastructure.Persistence.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
        
        
    }
}
