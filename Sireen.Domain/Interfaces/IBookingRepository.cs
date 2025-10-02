using Sireen.Domain.Enums;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task SoftDeleteAsync(int id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Booking>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync();
        Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task UpdateStatusAsync(int bookingId, BookingStatus status);
        Task CancelAsync(int bookingId);
    }
}
