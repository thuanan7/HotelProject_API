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
        public async Task<List<Hotel>> GetAll(Expression<Func<Hotel, bool>>? filter = null)
        {
            IQueryable<Hotel> query = _context.Hotels;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Hotel> Get(Expression<Func<Hotel, bool>>? filter = null, bool tracked = true)
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

        

        public async Task Create(Hotel entity)
        {
            await _context.AddAsync(entity);
            await Save();
        }

        public async Task Remove(Hotel entity)
        {
            _context.Remove(entity);
            await Save();
        }

        public async Task Update(Hotel entity)
        {
            _context.Update(entity);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
