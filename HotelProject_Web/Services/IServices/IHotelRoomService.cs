using HotelProject_Web.Models.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IHotelRoomService
    {
        Task<T> GetALlAsync<T>(string token);
        Task<T> GetAsync<T>(int hotelId, int roomNo, string token);
        Task<T> CreateAsync<T>(CreateHotelRoomDTO dto, string token);
        Task<T> UpdateAsync<T>(UpdateHotelRoomDTO dto, string token);
        Task<T> DeleteAsync<T>(int hotelId, int roomNo, string token);
    }
}
