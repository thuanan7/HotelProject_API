using HotelProject_HotelAPI.Models;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace HotelProject_HotelAPI.Repository
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll(Expression<Func<Hotel, bool>>? filter = null);
        Task<Hotel> Get(Expression<Func<Hotel, bool>>? filter = null, bool tracked = true);
        Task Create(Hotel entity);
        Task Remove(Hotel entity);
        Task Update(Hotel entity);
        Task Save();
    }
}
