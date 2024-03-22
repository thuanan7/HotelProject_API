using HotelProject_Web.DTO;

namespace HotelProject_Web.Services.IServices
{
    public interface IHotelRoomService
    {
        Task<T> GetALlAsync<T>();
        Task<T> GetAsync<T>(int hotelId, int roomNo);
        Task<T> CreateAsync<T>(CreateHotelRoomDTO dto);
        Task<T> UpdateAsync<T>(UpdateHotelRoomDTO dto);
        Task<T> DeleteAsync<T>(int hotelId, int roomNo);
    }
}
