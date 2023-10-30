using ApplicationCore.Domain.Entities.Room;
using ApplicationCore.Domain.Repositories.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class RoomTypeRepository : SimpleRepository<RoomType, int>,IRoomTypeRepository
{
    public RoomTypeRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<RoomType> GetByNameAsync(string name)
    {
        return await DbContext.Set<RoomType>().FirstOrDefaultAsync(e => e.Name == name);
    }
}