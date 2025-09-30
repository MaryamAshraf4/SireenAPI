using Microsoft.EntityFrameworkCore;
using Sireen.Domain.Interfaces;
using Sireen.Domain.Models;
using Sireen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.Select(h => new Hotel
            {
                Id = h.Id,
                Name = h.Name,
                Location = h.Location,
                Description = h.Description,
                HotelImages = h.HotelImages.ToList()

            }).ToListAsync();   
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels.Include(h => h.Rooms)
                .Include(h => h.HotelImages).Include(h => h.Ratings)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByManagerIdAsync(string managerId)
        {
            return await _context.Hotels.Where(h => h.ManagerId == managerId).ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> SearchAsync(string? name, string? location)
        {
            var query = _context.Hotels.AsQueryable();

            if(!string.IsNullOrEmpty(name))
                query = query.Where(h => h.Name.Contains(name));

            if(!string.IsNullOrEmpty(location))
                query = query.Where(h => h.Location.Contains(location));

            return await query.ToListAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if(hotel != null)
            {
                hotel.IsDeleted = true;
            }
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            var existingHotel = await _context.Hotels.FindAsync(hotel.Id);

            if (existingHotel is null) return;
            
            existingHotel.Name = hotel.Name;
            existingHotel.Email = hotel.Email;
            existingHotel.UpdatedAt = DateTime.Now;
            existingHotel.Location = hotel.Location;
            existingHotel.PhoneNumber = hotel.PhoneNumber;
            existingHotel.Description = hotel.Description;
        }
    }
}
