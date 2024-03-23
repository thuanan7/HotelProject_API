using HotelProject_HotelAPI.Data;
using HotelProject_HotelAPI.Models;
using HotelProject_HotelAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelProject_HotelAPI.Repository
{
    public class HotelRoomRepository : Repository<HotelRoom>, IHotelRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<HotelRoom> UpdateAsync(HotelRoom entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
