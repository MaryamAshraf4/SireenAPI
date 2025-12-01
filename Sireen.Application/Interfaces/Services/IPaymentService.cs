using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Payments;
using Sireen.Application.DTOs.Ratings;
using Sireen.Application.Helpers;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto?> GetByIdAsync(int id);      
        Task<IEnumerable<PaymentDto>> GetByUserIdAsync(string userId);
        Task<IEnumerable<PaymentDto>> GetByBookingIdAsync(int bookingId);
        Task<ServiceResult> AddAsync(CreatePaymentDto paymentDto, int bookingId);
        Task<ServiceResult> UpdatePaymentStatusAsync(int paymentId, UpdatePaymentManagerDto paymentDto);
        Task<IEnumerable<PaymentDto>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate);
    }
}
