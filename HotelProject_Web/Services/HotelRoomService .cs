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

        public Task<T> CreateAsync<T>(CreateHotelRoomDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int hotelId, int roomNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}",
                Token = token
            });
        }

        public Task<T> GetALlAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int hotelId, int roomNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{hotelId}/rooms/{roomNo}",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(UpdateHotelRoomDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelRoomApi/hotels/{dto.HotelId}/rooms/{dto.RoomNo}",
                Token = token
            });
        }
    }
}
