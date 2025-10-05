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
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;
        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
        }

        public async Task<double> GetAverageRatingByHotelAsync(int hotelId)
        {
            var ratings = await _context.Ratings.Where(r => r.HotelId == hotelId).ToListAsync();

            return ratings.Any() ? ratings.Average(r => r.Score) : 0.0;
        }

        public async Task<IEnumerable<Rating>> GetRatingsByHotelAsync(int hotelId)
        {
            return await _context.Ratings.Include(r => r.User).Where(r => r.HotelId  == hotelId).ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByUserAsync(string userId)
        {
            return await _context.Ratings.Include(r => r.Hotel).Where(r => r.UserId == userId) .ToListAsync();
        }
    }
}
