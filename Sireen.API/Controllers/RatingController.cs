using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Ratings;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Models;
using System.Security.Claims;

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
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRatingByHotel(int hotelId) 
        {
            var result = await _ratingService.GetAverageRatingByHotelAsync(hotelId);

            return Ok(result);
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRatingsByHotel(int hotelId) 
        {
            var result = await _ratingService.GetRatingsByHotelAsync(hotelId);

            return Ok(result);
        }

        [HttpGet("GetRatingsByUser")]
        [Authorize]
        public async Task<IActionResult> GetRatingsByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("Unauthorized User");

            var result = await _ratingService.GetRatingsByUserAsync(userId);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddRating([FromBody]CreateRatingDto ratingDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("Unauthorized User");

            var result = await _ratingService.AddAsync(ratingDto, userId);

            return Ok(result);
        }
    }
}
