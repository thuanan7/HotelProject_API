using HotelProject_Web.Models.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IHotelService
    {
        Task<T> GetALlAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CreateHotelDTO dto);
        Task<T> UpdateAsync<T>(HotelDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
