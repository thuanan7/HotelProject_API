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

        public Task<T> CreateAsync<T>(CreateHotelDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi",
                Token = token,
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{id}",
                Token = token
            });
        }

        public Task<T> GetALlAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{id}",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(UpdateHotelDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{dto.Id}",
                Token = token,
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
