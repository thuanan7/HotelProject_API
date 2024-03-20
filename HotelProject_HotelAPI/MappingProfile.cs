using AutoMapper;
using HotelProject_HotelAPI.DTO;
using HotelProject_HotelAPI.Models;

namespace HotelProject_HotelAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelDTO>();
            CreateMap<HotelDTO, Hotel>();

            CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
        }
    }
}
