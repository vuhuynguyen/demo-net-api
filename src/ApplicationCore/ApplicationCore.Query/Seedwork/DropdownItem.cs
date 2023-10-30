using ApplicationCore.Domain.Entities.Room;
using AutoMapper;

namespace ApplicationCore.Query.Seedwork
{
    public class DropdownItem<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }

    class DropdownItemMappingProfile : Profile
    {
        public DropdownItemMappingProfile()
        {
            CreateMap<RoomType, DropdownItem<int>>();
        }
    }
}
