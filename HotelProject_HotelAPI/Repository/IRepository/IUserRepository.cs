using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using System.Diagnostics.Eventing.Reader;

namespace HotelProject_HotelAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
    }
}
