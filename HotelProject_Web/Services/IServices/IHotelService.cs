using HotelProject_Web.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IHotelService
    {
        Task<T> GetALlAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CreateHotelDTO dto);
        Task<T> UpdateAsync<T>(int id, HotelDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
