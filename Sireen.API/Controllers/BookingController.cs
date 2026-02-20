using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Bookings;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Enums;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var result = await _bookingService.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("GetActive")]
        public async Task<IActionResult> GetActiveBookingsAsync()
        {
            var result = await _bookingService.GetActiveBookingsAsync();

            return Ok(result);
        }

        [HttpGet("GetStatus/{status}")]
        public async Task<IActionResult> GetByStatus(BookingStatus status)
        {
            var result = await _bookingService.GetByStatusAsync(status);

            return Ok(result);
        }

        [HttpGet("Client/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _bookingService.GetByUserIdAsync(userId);

            return Ok(result);
        }

        [HttpGet("Room/{roomId}")]
        public async Task<IActionResult> GetByRoomIdAsync(int roomId)
        {
            var result = await _bookingService.GetByRoomIdAsync(roomId);

            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var room = await _bookingService.GetByIdAsync(id);

            if (room == null)
                return NotFound("Booking not found.");

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(CreateBookingDto bookingDto, [FromQuery] string clientId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookingService.AddAsync(bookingDto, clientId);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateManagerBookingDto bookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookingService.UpdateBookingAsync(id, bookingDto);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatus bookingStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _bookingService.UpdateStatusAsync(id, bookingStatus);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.SoftDeleteAsync(id);
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
