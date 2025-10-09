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
    public class AmenityRepository : GenericRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Amenity>> GetAllAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<IEnumerable<Amenity>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Amenities.Where(a => ids.Contains(a.Id)).ToListAsync();
        }
    }
}
