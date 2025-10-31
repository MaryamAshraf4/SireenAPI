using Sireen.Application.DTOs.Ratings;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RatingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult> AddAsync(CreateRatingDto ratingDto, string userId)
        {
            var rating = new Rating { 
                Score = ratingDto.Score,
                Comment = ratingDto.Comment,
                HotelId = ratingDto.HotelId,
                UserId = userId
            };

            await _unitOfWork.Ratings.AddRatingAsync(rating);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Rating added successfully.");
        }

        public async Task<double> GetAverageRatingByHotelAsync(int hotelId)
        {
            return await _unitOfWork.Ratings.GetAverageRatingByHotelAsync(hotelId);
        }

        public async Task<IEnumerable<HotelRatingDto>> GetRatingsByHotelAsync(int hotelId)
        {
            var ratings = await _unitOfWork.Ratings.GetRatingsByHotelAsync(hotelId);

            return ratings.Select(r => new HotelRatingDto {
                Id = r.Id,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                ClientFullName = r.User.FullName,
                ClientNationality = r.User.Nationality
            });
        }

        public async Task<IEnumerable<ClientRatingDto>> GetRatingsByUserAsync(string userId)
        {
            var ratings = await _unitOfWork.Ratings.GetRatingsByUserAsync(userId);

            return ratings.Select(r => new ClientRatingDto
            {
                Id = r.Id,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                HotelName = r.Hotel.Name
            });
        }
    }
}
