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

namespace Sireen.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> AddAsync(CreateBookingDto bookingDto, string clientId)
        {
            bool available = await _unitOfWork.Bookings.IsRoomAvailableAsync(bookingDto.RoomId, bookingDto.CheckIn, bookingDto.CheckOut);

            if (!available)
                return ServiceResult.FailureResult("Room is not available for selected dates.");

            var booking = new Booking
            {
                UserId = clientId,
                RoomId = bookingDto.RoomId,
                CheckIn = bookingDto.CheckIn,
                CheckOut = bookingDto.CheckOut,
                Payment = new Payment
                {
                    AmountPaid = bookingDto.PaymentCreate.AmountPaid,
                    PaymentMethod = bookingDto.PaymentCreate.PaymentMethod,
                }
            };

            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Booking created successfully.");
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetActiveBookingsAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetActiveBookingsAsync();

            return bookings.Select(b => new ManagerBookingDto
            {
                Id = b.Id,
                RoomNumber = b.Room.RoomNumber,
                BookingStatus = b.BookingStatus,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                TotalAmount = b.Payment.AmountPaid,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id = b.Payment.Id,
                    AmountPaid = b.Payment.AmountPaid,
                    PaymentStatus = b.Payment.PaymentStatus,
                    PaymentMethod = b.Payment.PaymentMethod,
                    PaymentDate = b.Payment.PaymentDate
                },
                Client = new ClientDto
                {
                    FullName = b.User.FullName,
                    PhoneNumber = b.User.PhoneNumber.ToString(),
                    Nationality = b.User.Nationality,
                    IdentityNumber = b.User.IdentityNumber,
                    IdentityExpiryDate = b.User.IdentityExpiryDate,
                    IdentityType = b.User.IdentityType
                }
            }).ToList();
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetAllAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync();

            return bookings.Select(b => new ManagerBookingDto
            {
                Id = b.Id,
                RoomNumber = b.Room.RoomNumber,
                BookingStatus = b.BookingStatus,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                TotalAmount = b.Payment.AmountPaid,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id=b.Payment.Id,
                    AmountPaid = b.Payment.AmountPaid,
                    PaymentStatus = b.Payment.PaymentStatus,
                    PaymentMethod = b.Payment.PaymentMethod,
                    PaymentDate = b.Payment.PaymentDate
                },
                Client = new ClientDto 
                {
                    FullName = b.User.FullName,
                    PhoneNumber = b.User.PhoneNumber.ToString(),
                    Nationality = b.User.Nationality,
                    IdentityNumber = b.User.IdentityNumber,
                    IdentityExpiryDate = b.User.IdentityExpiryDate,
                    IdentityType = b.User.IdentityType
                }
            }).ToList();
        }

        public async Task<ManagerBookingDto?> GetByIdAsync(int id)
        {
            var bookings = await _unitOfWork.Bookings.GetByIdAsync(id);

            return new ManagerBookingDto
            {
                Id = bookings.Id,
                RoomNumber = bookings.Room.RoomNumber,
                BookingStatus = bookings.BookingStatus,
                CheckIn = bookings.CheckIn,
                CheckOut = bookings.CheckOut,
                TotalAmount = bookings.Payment.AmountPaid,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id = bookings.Payment.Id,
                    AmountPaid = bookings.Payment.AmountPaid,
                    PaymentStatus = bookings.Payment.PaymentStatus,
                    PaymentMethod = bookings.Payment.PaymentMethod,
                    PaymentDate = bookings.Payment.PaymentDate
                },
                Client = new ClientDto
                {
                    FullName = bookings.User.FullName,
                    PhoneNumber = bookings.User.PhoneNumber.ToString(),
                    Nationality = bookings.User.Nationality,
                    IdentityNumber = bookings.User.IdentityNumber,
                    IdentityExpiryDate = bookings.User.IdentityExpiryDate,
                    IdentityType = bookings.User.IdentityType
                }
            };
        }

        public async Task<IEnumerable<ManagerBookingDto>> GetByRoomIdAsync(int roomId)
        {
            var bookings = await _unitOfWork.Bookings.GetByRoomIdAsync(roomId);

            return bookings.Select(b => new ManagerBookingDto
            {
                Id = b.Id,
                RoomNumber = b.Room.RoomNumber,
                BookingStatus = b.BookingStatus,
                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,
                TotalAmount = b.Payment.AmountPaid,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id = b.Payment.Id,
                    AmountPaid = b.Payment.AmountPaid,
                    PaymentStatus = b.Payment.PaymentStatus,
                    PaymentMethod = b.Payment.PaymentMethod,
                    PaymentDate = b.Payment.PaymentDate
                },
                Client = new ClientDto
                {
                    FullName = b.User.FullName,
                    PhoneNumber = b.User.PhoneNumber.ToString(),
                    Nationality = b.User.Nationality,
                    IdentityNumber = b.User.IdentityNumber,
                    IdentityExpiryDate = b.User.IdentityExpiryDate,
                    IdentityType = b.User.IdentityType
                }
            }).ToList();
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByStatusAsync(BookingStatus status)
        {
            var bookings = await _unitOfWork.Bookings.GetByStatusAsync(status);

            return bookings.Select(c => new ClientBookingDto
            {
                Id = c.Id,
                RoomNumber = c.Room.RoomNumber,
                BookingStatus = c.BookingStatus,
                CheckIn = c.CheckIn,
                CheckOut = c.CheckOut,
                TotalAmount = c.Payment.AmountPaid,
                HotelLocation = c.Room.Hotel.Location,
                HotelName = c.Room.Hotel.Name,
                HotelPhoneNumber = c.Room.Hotel.PhoneNumber,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id = c.Payment.Id,
                    AmountPaid = c.Payment.AmountPaid,
                    PaymentStatus = c.Payment.PaymentStatus,
                    PaymentMethod = c.Payment.PaymentMethod,
                    PaymentDate = c.Payment.PaymentDate
                }
            }).ToList();
        }

        public async Task<IEnumerable<ClientBookingDto>> GetByUserIdAsync(string userId)
        {
            var bookings = await _unitOfWork.Bookings.GetByUserIdAsync(userId);

            return bookings.Select(c => new ClientBookingDto
            {
                Id = c.Id,
                RoomNumber = c.Room.RoomNumber,
                BookingStatus = c.BookingStatus,
                CheckIn = c.CheckIn,
                CheckOut = c.CheckOut,
                TotalAmount = c.Payment.AmountPaid,
                HotelLocation = c.Room.Hotel.Location,
                HotelName = c.Room.Hotel.Name,
                HotelPhoneNumber = c.Room.Hotel.PhoneNumber,
                Payment = new PaymentDisplayForBookingDto
                {
                    Id = c.Payment.Id,
                    AmountPaid = c.Payment.AmountPaid,
                    PaymentStatus = c.Payment.PaymentStatus,
                    PaymentMethod = c.Payment.PaymentMethod,
                    PaymentDate = c.Payment.PaymentDate
                }
            }).ToList();
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

            if(bookingDto.CheckIn != null)
            booking.CheckIn = bookingDto.CheckIn.Value;
           
            if(bookingDto.CheckOut != null)
            booking.CheckIn = bookingDto.CheckOut.Value;
            
            if(bookingDto.BookingStatus != null)
            booking.BookingStatus = bookingDto.BookingStatus.Value;
           
            if(bookingDto.RoomId != null)
            booking.RoomId = bookingDto.RoomId.Value;

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
