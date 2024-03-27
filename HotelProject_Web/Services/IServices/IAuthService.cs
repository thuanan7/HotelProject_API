using HotelProject_Web.Models.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegisterRequestDTO objToCreate);
        Task<T> LogoutAsync<T>(TokenDTO obj);
    }
}
