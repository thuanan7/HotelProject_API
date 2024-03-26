using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class AuthService : IAuthService
    {

        private string hotelApiUrl;
        private readonly IBaseService _baseService;
        public AuthService(IConfiguration configuration, IBaseService baseService)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
            _baseService = baseService;
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = hotelApiUrl + "/api/User/login"
            });
        }

        public async Task<T> RegisterAsync<T>(RegisterRequestDTO objToCreate)
        {
            return await _baseService.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = hotelApiUrl + "/api/User/register"
            });
        }
    }
}
