using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class HotelService : BaseService, IHotelService
    {
        private string hotelApiUrl;
        public HotelService(IHttpClientFactory clienFactory, IConfiguration configuration) : base(clienFactory)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
        }

        public Task<T> CreateAsync<T>(CreateHotelDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + "/api/HotelApi"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/HotelApi/{id}"
            });
        }

        public Task<T> GetALlAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + "/api/HotelApi"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/HotelApi/{id}"
            });
        }

        public Task<T> UpdateAsync<T>(HotelDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/HotelApi/{dto.Id}"
            });
        }
    }
}
