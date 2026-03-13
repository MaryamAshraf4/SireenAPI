using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Bookings;
using Sireen.Application.DTOs.Payments;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Enums;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResult> AddAsync(CreateBookingDto bookingDto, string clientId)
        {
            bool available = await _unitOfWork.Bookings.IsRoomAvailableAsync(bookingDto.RoomId, bookingDto.CheckIn, bookingDto.CheckOut);

            if (!available)
                return ServiceResult.FailureResult("Room is not available for selected dates.");

            var booking = _mapper.Map<Booking>(bookingDto);
            booking.UserId = clientId;

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking created successfully.");
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetActiveBookingsAsync(string managerId)
        {
            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return Enumerable.Empty<ManagerBookingDto>();

            var bookings = await _unitOfWork.Bookings.GetActiveBookingsAsync();

            var managerBookings = bookings.Where(b => user.Hotels.Any(h => h.Id == b.Room.HotelId));

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(managerBookings);
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetAllAsync(string managerId)
        {
            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return Enumerable.Empty<ManagerBookingDto>();

            var bookings = await _unitOfWork.Bookings.GetAllAsync();

            var managerBookings = bookings.Where(b => user.Hotels.Any(h => h.Id == b.Room.HotelId));

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(managerBookings);
        }

        public async Task<ManagerBookingDto?> GetByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);

            return _mapper.Map<ManagerBookingDto>(booking);
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetByRoomIdAsync(int roomId, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);

            if (room == null || room.IsDelete)
                return Enumerable.Empty<ManagerBookingDto>();

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return Enumerable.Empty<ManagerBookingDto>();

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return Enumerable.Empty<ManagerBookingDto>();

            var bookings = await _unitOfWork.Bookings.GetByRoomIdAsync(roomId);

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(bookings);
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByStatusAsync(BookingStatus status, string managerId)
        {
            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return Enumerable.Empty<ClientBookingDto>();

            var bookings = await _unitOfWork.Bookings.GetByStatusAsync(status);

            var managerBookings = bookings.Where(b => user.Hotels.Any(h => h.Id == b.Room.HotelId));

            return _mapper.Map<IEnumerable<ClientBookingDto>>(bookings);
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByUserIdAsync(string userId)
        {
            var bookings = await _unitOfWork.Bookings.GetByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<ClientBookingDto>>(bookings);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id, string managerId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            if (booking.IsDeleted)
                return ServiceResult.FailureResult("Booking already deleted.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == booking.Room.Hotel.Id))
                return ServiceResult.FailureResult("You cannot delete this booking.");

            booking.IsDeleted = true;
            _unitOfWork.Bookings.Update(booking);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking deleted successfully.");
        }

        public async Task<ServiceResult> UpdateBookingAsync(int bookingId, UpdateManagerBookingDto bookingDto, string managerId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == booking.Room.Hotel.Id))
                return ServiceResult.FailureResult("You cannot update this booking.");

            _mapper.Map(bookingDto, booking);

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking updated successfully");
        }

        public async Task<ServiceResult> UpdateStatusAsync(int bookingId, BookingStatus status, string managerId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == booking.Room.Hotel.Id))
                return ServiceResult.FailureResult("You cannot update status for this booking.");

            booking.BookingStatus = status;

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking status updated successfully");
        }
    }
}
