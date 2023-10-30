using ApplicationCore.Domain.Entities.Room;
using ApplicationCore.Domain.SeedWork;

namespace ApplicationCore.Domain.Repositories.Rooms
{
    public interface IRoomTypeRepository : ISimpleRepository<RoomType, int>
    {
        Task<RoomType> GetByNameAsync(string name);
    }
}