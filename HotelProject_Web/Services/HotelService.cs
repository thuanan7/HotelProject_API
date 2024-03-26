using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class HotelService : IHotelService
    {
        private string hotelApiUrl;
        private readonly IBaseService _baseService;
        public HotelService(IConfiguration configuration, IBaseService baseService)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
            _baseService = baseService;
        }

        public async Task<T> CreateAsync<T>(CreateHotelDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{id}",
            });
        }

        public async Task<T> GetALlAsync<T>()
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{id}",
            });
        }

        public async Task<T> UpdateAsync<T>(UpdateHotelDTO dto)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = hotelApiUrl + $"/api/{SD.CurrentAPIVersion}/HotelApi/{dto.Id}",
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
