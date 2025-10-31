using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.Interfaces.Services;
using Sireen.Application.Services;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService _amenityService;
        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAmenities() 
        {
            var result = await _amenityService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenityById(int id) 
        {
            var amenity = await _amenityService.GetByIdAsync(id);

            if (amenity == null)
                return NotFound("Amenity not found.");

            return Ok(amenity);
        }

        [HttpPost]
        public async Task<IActionResult> AddAmenity(CreateAmenityDto amenityDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _amenityService.AddAsync(amenityDto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmenity(int id, [FromBody]UpdateAmenityDto amenityDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _amenityService.UpdateAmenityAsync(id, amenityDto);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }
    }
}
