using AutoMapper;
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
        private readonly IMapper _mapper;
        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult> AddAsync(CreateRatingDto ratingDto, string userId)
        {
            var rating = _mapper.Map<Rating>(ratingDto); 
            rating.UserId = userId;     

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

            return _mapper.Map<IEnumerable<HotelRatingDto>>(ratings);
        }

        public async Task<IEnumerable<ClientRatingDto>> GetRatingsByUserAsync(string userId)
        {
            var ratings = await _unitOfWork.Ratings.GetRatingsByUserAsync(userId);

            return _mapper.Map<IEnumerable<ClientRatingDto>>(ratings);
        }
    }
}
