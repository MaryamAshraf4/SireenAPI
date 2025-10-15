using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces.Repository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Payment>> GetByBookingIdAsync(int bookingId);
        Task<IEnumerable<Payment>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate);
    }
}
