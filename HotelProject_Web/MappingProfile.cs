using AutoMapper;
using HotelProject_Web.Models.DTO;

namespace HotelProject_Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelDTO, CreateHotelDTO>().ReverseMap();
            CreateMap<HotelDTO, UpdateHotelDTO>().ReverseMap();

            CreateMap<HotelRoomDTO, CreateHotelRoomDTO>().ReverseMap();
            CreateMap<HotelRoomDTO, UpdateHotelRoomDTO>().ReverseMap();
        }
    }
}
