using HotelProject_HotelAPI.Models;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace HotelProject_HotelAPI.Repository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAllAsync(Expression<Func<Hotel, bool>>? filter = null);
        Task<Hotel> GetAsync(Expression<Func<Hotel, bool>>? filter = null, bool tracked = true);
        Task CreateAsync(Hotel entity);
        Task RemoveAsync(Hotel entity);
        Task UpdateAsync(Hotel entity);
        Task SaveAsync();
    }
}
