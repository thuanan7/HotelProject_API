using HotelProject_Utility;
using HotelProject_Web.Models;
using HotelProject_Web.Models.DTO;
using HotelProject_Web.Services.IServices;

namespace HotelProject_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private string hotelApiUrl;
        public AuthService(IHttpClientFactory clienFactory, IConfiguration configuration) : base(clienFactory)
        {
            hotelApiUrl = configuration.GetValue<string>("ServiceUrls:HotelProjectAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = hotelApiUrl + "/api/v1/User/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegisterRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = hotelApiUrl + "/api/v1/User/register"
            });
        }
    }
}
