using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.API.DTOs.HotelImages;
using Sireen.API.DTOs.RoomImages;
using Sireen.API.Interfaces.IService;
using Sireen.API.Service;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.Interfaces.Services;
using System.Security.Claims;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IHotelImageService _hotelImageService;
        public HotelController(IHotelService hotelService, IHotelImageService hotelImageService)
        {
            _hotelService = hotelService;
            _hotelImageService = hotelImageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHotels()
        { 
            var result = await _hotelService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelById(int id)
        { 
            var hotel = await _hotelService.GetByIdAsync(id);

            if (hotel == null)
                return NotFound("Hotel not found.");

            return Ok(hotel);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchHotels([FromQuery]string? name, [FromQuery]string? location)
        { 
            var hotel = await _hotelService.SearchAsync(name, location);

            return Ok(hotel);
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByManager()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var hotels = await _hotelService.GetHotelsByManagerIdAsync(managerId);

            return Ok(hotels);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddHotel(CreateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _hotelService.AddAsync(hotelDto, managerId);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody]UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _hotelService.UpdateHotelAsync(id, hotelDto, managerId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _hotelService.SoftDeleteAsync(id, managerId);
            if (!result.Success)
            {
                if(result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }              

            return Ok(result.Message);
        }

        [HttpPost("hotels/{hotelId}/images")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UploadHotelImage(int hotelId, [FromForm] HotelImageUploadDto dto)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            dto.HotelId = hotelId;

            string url = await _hotelImageService.AddHotelImage(dto, managerId);

            return Ok(new { imageUrl = url });
        }

        [HttpGet("hotels/{hotelId}/images")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImagesByHotelIdAsync(int hotelId)
        {
            var result = await _hotelImageService.GetByHotelIdAsync(hotelId);

            return Ok(result);
        }

        [HttpDelete("DeleteImage/{imageId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteHotelImage(int imageId)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _hotelImageService.SoftDeleteAsync(imageId, managerId);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
