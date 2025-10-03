using Microsoft.EntityFrameworkCore;
using Sireen.Domain.Enums;
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
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task CancelAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking != null)
            {
                booking.BookingStatus = BookingStatus.Cancelled;
            }
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
        {
            return await _context.Bookings.Where(b => b.BookingStatus == BookingStatus.Confirmed).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.Include(b => b.User)
                .Include(b => b.Payment).Include(b => b.Room).ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.Include(b => b.User)
                .Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId)
        {
            return await _context.Bookings.Where(b => b.RoomId == roomId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status)
        {
            return await _context.Bookings.Where(b => b.BookingStatus == status).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(string userId)
        {
            return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return !await _context.Bookings.AnyAsync(
                b => b.RoomId == roomId && 
                (checkIn < b.CheckOut && checkOut > b.CheckIn) || (b.CheckOut == null && checkOut > b.CheckIn) );
        }

        public async Task SoftDeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                booking.IsDeleted = true;
            }
        }

        public async Task UpdateStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking != null)
            {
                booking.BookingStatus = status;
            }
        }
    }
}
