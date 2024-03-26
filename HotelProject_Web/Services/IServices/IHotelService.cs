using HotelProject_Web.Models.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IHotelService
    {
        Task<T> GetALlAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CreateHotelDTO dto, string token);
        Task<T> UpdateAsync<T>(UpdateHotelDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
