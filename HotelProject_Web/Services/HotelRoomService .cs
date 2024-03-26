using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class HotelRoomService : IHotelRoomService
    {
        private string hotelApiUrl;
        private readonly IBaseService _baseService;
        public HotelRoomService(IConfiguration configuration, IBaseService baseService)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(CreateHotelRoomDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi"
            });
        }

        public async Task<T> DeleteAsync<T>(int hotelId, int roomNo)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}"
            });
        }

        public async Task<T> GetALlAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi"
            });
        }

        public async Task<T> GetAsync<T>(int hotelId, int roomNo)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}"
            });
        }

        public async Task<T> UpdateAsync<T>(UpdateHotelRoomDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{dto.HotelId}/rooms/{dto.RoomNo}"
            });
        }
    }
}
