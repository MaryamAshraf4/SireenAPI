using Sireen.Application.DTOs.Ratings;
using Sireen.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IRatingService
    {
        Task<ServiceResult> AddAsync(CreateRatingDto ratingDto, string userId);
        Task<double> GetAverageRatingByHotelAsync(int hotelId);
        Task<IEnumerable<HotelRatingDto>> GetRatingsByHotelAsync(int hotelId);
        Task<IEnumerable<ClientRatingDto>> GetRatingsByUserAsync(string userId);
    }
}
