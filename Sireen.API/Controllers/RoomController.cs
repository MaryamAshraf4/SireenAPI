using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
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
    }
}
