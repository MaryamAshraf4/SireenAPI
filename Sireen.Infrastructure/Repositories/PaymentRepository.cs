using Microsoft.EntityFrameworkCore;
using Sireen.Domain.Interfaces;
using Sireen.Domain.Models;
using Sireen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Payments.Where(b => b.BookingId == bookingId).ToListAsync();
        }

        public override async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Booking)
                .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(string userId)
        {
            return await _context.Payments.Include(p => p.Booking)
                .Where(p => p.Booking.UserId  == userId).ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Payments.Include(p => p.Booking)
                .ThenInclude(b => b.Room).ThenInclude(r => r.Hotel).Where(p => p.Booking.Room.HotelId == hotelId);

            if (startDate.HasValue)
                query = query.Where(p => p.PaymentDate >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(p => p.PaymentDate <= endDate.Value.Date.AddDays(1));

            return await query.ToListAsync();
        }
    }
}
