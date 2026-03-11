using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.Bookings;
using Sireen.Application.DTOs.Payments;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Enums;
using System.Security.Claims;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpGet("Booking/{bookingId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPaymentByBookingId(int bookingId)
        {
            var result = await _paymentService.GetByBookingIdAsync(bookingId);

            return Ok(result);
        }

        [HttpGet("GetPaymentByUserId")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPaymentByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized("Unauthorized User");

            var result = await _paymentService.GetByUserIdAsync(userId);

            return Ok(result);
        }

        [HttpGet("Hotel/{hotelId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetPaymentsByHotelAndDate(int hotelId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var result = await _paymentService.GetPaymentsByHotelAndDateAsync(hotelId, startDate, endDate);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPayment(CreatePaymentDto paymentDto, [FromQuery] int bookingId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _paymentService.AddAsync(paymentDto, bookingId);

            return Ok(result);
        }

        [HttpPut("{paymentId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateStatus(int paymentId, UpdatePaymentManagerDto paymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (managerId == null)
                return Unauthorized("Unauthorized User");

            var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, paymentDto, managerId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }
    }
}
