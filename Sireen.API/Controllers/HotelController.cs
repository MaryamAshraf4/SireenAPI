using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.API.DTOs.HotelImages;
using Sireen.API.DTOs.RoomImages;
using Sireen.API.Interfaces.IService;
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
        public async Task<IActionResult> GetAllHotels()
        { 
            var result = await _hotelService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        { 
            var hotel = await _hotelService.GetByIdAsync(id);

            if (hotel == null)
                return NotFound("Hotel not found.");

            return Ok(hotel);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchHotels([FromQuery]string? name, [FromQuery]string? location)
        { 
            var hotel = await _hotelService.SearchAsync(name, location);

            return Ok(hotel);
        }

        [HttpGet("manager")]
        public async Task<IActionResult> GetByManager( [FromQuery]string managerId)
        {           
            var hotels = await _hotelService.GetHotelsByManagerIdAsync(managerId);

            return Ok(hotels);
        }

        [HttpPost]
        public async Task<IActionResult> AddHotel(CreateHotelDto hotelDto,[FromQuery] string managerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _hotelService.AddAsync(hotelDto, managerId);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody]UpdateHotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _hotelService.UpdateHotelAsync(id, hotelDto);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var result = await _hotelService.SoftDeleteAsync(id);
            if (!result.Success)
            {
                if(result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }              

            return Ok(result.Message);
        }

        [HttpPost("hotels/{hotelId}/images")]
        public async Task<IActionResult> UploadHotelImage(int hotelId, [FromForm] HotelImageUploadDto dto)
        {
            dto.HotelId = hotelId;

            string url = await _hotelImageService.AddHotelImage(dto);

            return Ok(new { imageUrl = url });
        }

        [HttpGet("hotels/{hotelId}/images")]
        public async Task<IActionResult> GetImagesByHotelIdAsync(int hotelId)
        {
            var result = await _hotelImageService.GetByHotelIdAsync(hotelId);

            return Ok(result);
        }
    }
}
