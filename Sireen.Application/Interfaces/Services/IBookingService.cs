using Sireen.Application.DTOs.Bookings;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Domain.Enums;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<ManagerBookingDto?> GetByIdAsync(int id);
        Task<ServiceResult> SoftDeleteAsync(int id, string managerId);
        Task<IEnumerable<ManagerBookingDto>> GetAllAsync();
        Task<IEnumerable<ManagerBookingDto>> GetActiveBookingsAsync();
        Task<IEnumerable<ManagerBookingDto>> GetByRoomIdAsync(int roomId);
        Task<IEnumerable<ClientBookingDto>> GetByUserIdAsync(string userId);
        Task<ServiceResult> UpdateStatusAsync(int bookingId, BookingStatus status, string managerId);
        Task<ServiceResult> UpdateBookingAsync(int bookingId, UpdateManagerBookingDto bookingDto, string managerId);
        Task<IEnumerable<ClientBookingDto>> GetByStatusAsync(BookingStatus status);
        Task<ServiceResult> AddAsync(CreateBookingDto bookingDto, string clientId);
    }
}
