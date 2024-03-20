using HotelProject_HotelAPI.Models;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace HotelProject_HotelAPI.Repository.IRepository
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel> UpdateAsync(Hotel entity);
    }
}
