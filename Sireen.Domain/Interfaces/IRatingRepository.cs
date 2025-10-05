using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(Rating rating);
        Task<double> GetAverageRatingByHotelAsync(int hotelId);
        Task<IEnumerable<Rating>> GetRatingsByHotelAsync(int hotelId);
        Task<IEnumerable<Rating>> GetRatingsByUserAsync(string userId);
    }
}
