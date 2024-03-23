using AutoMapper;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;

namespace HotelProject_HotelAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDTO>();
            CreateMap<HotelDTO, Hotel>();

            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();

            CreateMap<HotelRoom, HotelRoomDTO>().ReverseMap();
            CreateMap<HotelRoom, CreateHotelRoomDTO>().ReverseMap();
            CreateMap<HotelRoom, UpdateHotelRoomDTO>().ReverseMap();

            CreateMap<LocalUser, RegisterRequestDTO>().ReverseMap();
        }
    }
}
