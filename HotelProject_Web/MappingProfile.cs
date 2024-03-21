using AutoMapper;
using HotelProject_Web.DTO;

namespace HotelProject_Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelDTO, CreateHotelDTO>().ReverseMap();

            CreateMap<HotelRoomDTO, CreateHotelRoomDTO>().ReverseMap();
            CreateMap<HotelRoomDTO, UpdateHotelRoomDTO>().ReverseMap();
        }
    }
}
