using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task AddAsync(Payment payment);
        Task<IEnumerable<Payment>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Payment>> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<Payment>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate);
    }
}
