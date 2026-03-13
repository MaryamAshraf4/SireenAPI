using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Bookings;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Enums;
using System.Security.Claims;

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
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllBookings()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.GetAllAsync(managerId);

            return Ok(result);
        }

        [HttpGet("GetActive")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetActiveBookingsAsync()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.GetActiveBookingsAsync(managerId);

            return Ok(result);
        }

        [HttpGet("GetStatus/{status}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByStatus(BookingStatus status)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.GetByStatusAsync(status, managerId);

            return Ok(result);
        }

        [HttpGet("Client")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.GetByUserIdAsync(userId);

            return Ok(result);
        }

        [HttpGet("Room/{roomId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByRoomIdAsync(int roomId)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.GetByRoomIdAsync(roomId, managerId);

            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var room = await _bookingService.GetByIdAsync(id);

            if (room == null)
                return NotFound("Booking not found.");

            return Ok(room);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddBooking(CreateBookingDto bookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (clientId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.AddAsync(bookingDto, clientId);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateManagerBookingDto bookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.UpdateBookingAsync(id, bookingDto, managerId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpPut("status/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatus bookingStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.UpdateStatusAsync(id, bookingStatus, managerId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _bookingService.SoftDeleteAsync(id, managerId);
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
