using AutoMapper;
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
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<IEnumerable<ManagerBookingDto>> GetActiveBookingsAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetActiveBookingsAsync();

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(bookings);
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetAllAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync();

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(bookings);
        }

        public async Task<ManagerBookingDto?> GetByIdAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);

            return _mapper.Map<ManagerBookingDto>(booking);
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetByRoomIdAsync(int roomId)
        {
            var bookings = await _unitOfWork.Bookings.GetByRoomIdAsync(roomId);

            return _mapper.Map<IEnumerable<ManagerBookingDto>>(bookings);
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByStatusAsync(BookingStatus status)
        {
            var bookings = await _unitOfWork.Bookings.GetByStatusAsync(status);

            return _mapper.Map<IEnumerable<ClientBookingDto>>(bookings);
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByUserIdAsync(string userId)
        {
            var bookings = await _unitOfWork.Bookings.GetByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<ClientBookingDto>>(bookings);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(id);
            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            if (booking.IsDeleted)
                return ServiceResult.FailureResult("Booking already deleted.");

            booking.IsDeleted = true;
            _unitOfWork.Bookings.Update(booking);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking deleted successfully.");
        }

        public async Task<ServiceResult> UpdateBookingAsync(int bookingId, UpdateManagerBookingDto bookingDto)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            _mapper.Map(bookingDto, booking);

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking updated successfully");
        }

        public async Task<ServiceResult> UpdateStatusAsync(int bookingId, BookingStatus status)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);

            if (booking == null)
                return ServiceResult.FailureResult("Booking not found.");

            booking.BookingStatus = status;

            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking status updated successfully");
        }
    }
}
