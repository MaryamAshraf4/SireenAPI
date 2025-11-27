using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.API.DTOs.RoomImages;
using Sireen.API.Interfaces.IService;
using Sireen.API.Service;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Interfaces.Services;
using Sireen.Application.Services;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IRoomImageService _roomImageService;
        public RoomController(IRoomService roomService, IRoomImageService roomImageService)
        {
            _roomService = roomService;
            _roomImageService = roomImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms() 
        {
            var result = await _roomService.GetAllAsync();

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id) 
        {
            var room = await _roomService.GetByIdAsync(id);

            if (room == null)
                return NotFound("Room not found.");

            return Ok(room);
        }
        
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetRoomsByHotelIdAsync(int hotelId) 
        {
            var rooms = await _roomService.GetRoomsByHotelIdAsync(hotelId);

            return Ok(rooms);
        }

        [HttpGet("search/{hotelId}")]
        public async Task<IActionResult> SearchHotels([FromQuery] int? roomNumber, int hotelId)
        {
            var room = await _roomService.SearchAsync(roomNumber, hotelId);

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(CreateRoomDto roomDto, [FromQuery] int hotelId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.AddAsync(roomDto, hotelId);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roomService.UpdateRoomAsync(id, roomDto);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _roomService.SoftDeleteAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("{roomId}/amenities/{amenityId}")]
        public async Task<IActionResult> AddAmenityToRoom(int roomId, int amenityId)
        {
            var result = await _roomService.AddAmenityToRoomAsync(roomId, amenityId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{roomId}/amenities/{amenityId}")]
        public async Task<IActionResult> RemoveAmenityFromRoom(int roomId, int amenityId)
        {
            var result = await _roomService.RemoveAmenityFromRoomAsync(roomId, amenityId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("rooms/{roomId}/images")]
        public async Task<IActionResult> UploadRoomImage(int roomId, [FromForm] RoomImageUploadDto dto)
        {
            dto.RoomId = roomId;

            string url = await _roomImageService.AddRoomImage(dto);

            return Ok(new { imageUrl = url });
        }

        [HttpGet("rooms/{roomId}/images")]
        public async Task<IActionResult> GetImagesByRoomIdAsync(int roomId)
        {
            var result = await _roomImageService.GetByRoomIdAsync(roomId);

            return Ok(result);
        }
    }
}
