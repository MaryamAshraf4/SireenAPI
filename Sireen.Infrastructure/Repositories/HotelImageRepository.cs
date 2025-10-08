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
    public class HotelImageRepository : GenericRepository<HotelImage>, IHotelImageRepository
    {
        public HotelImageRepository(AppDbContext context) : base(context){}

        public async Task<IEnumerable<HotelImage>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.HotelImages.Where(hi => hi.HotelId == hotelId).ToListAsync();
        }
    }
}
