using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Models.DTO;
using System.Diagnostics.Eventing.Reader;

namespace HotelProject_HotelAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<RegisterResponseDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
    }
}
