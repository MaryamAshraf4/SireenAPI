using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Ratings;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Models;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("average/{hotelId}")]
        public async Task<IActionResult> GetAverageRatingByHotel(int hotelId) 
        {
            var result = await _ratingService.GetAverageRatingByHotelAsync(hotelId);

            return Ok(result);
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetRatingsByHotel(int hotelId) 
        {
            var result = await _ratingService.GetRatingsByHotelAsync(hotelId);

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRatingsByUser(string userId)
        {
            var result = await _ratingService.GetRatingsByUserAsync(userId);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody]CreateRatingDto ratingDto, [FromQuery]string userId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ratingService.AddAsync(ratingDto, userId);

            return Ok(result);
        }
    }
}
