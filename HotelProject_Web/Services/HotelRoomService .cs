using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class HotelRoomService : BaseService, IHotelRoomService
    {
        private string hotelApiUrl;
        public HotelRoomService(IHttpClientFactory clienFactory, IConfiguration configuration) : base(clienFactory)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
        }

        public Task<T> CreateAsync<T>(CreateHotelRoomDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + "/api/HotelRoomApi"
            });
        }

        public Task<T> DeleteAsync<T>(int hotelId, int roomNo)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}"
            });
        }

        public Task<T> GetALlAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + "/api/HotelRoomApi"
            });
        }

        public Task<T> GetAsync<T>(int hotelId, int roomNo)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}"
            });
        }

        public Task<T> UpdateAsync<T>(UpdateHotelRoomDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/HotelRoomApi/hotels/{dto.HotelId}/rooms/{dto.RoomNo}"
            });
        }
    }
}
