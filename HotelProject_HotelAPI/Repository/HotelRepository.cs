using HotelProject_HotelAPI.Controllers;
using HotelProject_HotelAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelProject_HotelAPI.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Hotel>> GetAllAsync(Expression<Func<Hotel, bool>>? filter = null)
        {
            IQueryable<Hotel> query = _context.Hotels;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Hotel> GetAsync(Expression<Func<Hotel, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<Hotel> query = _context.Hotels;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        

        public async Task CreateAsync(Hotel entity)
        {
            await _context.AddAsync(entity);
            await SaveAsync();
        }

        public async Task RemoveAsync(Hotel entity)
        {
            _context.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(Hotel entity)
        {
            _context.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
