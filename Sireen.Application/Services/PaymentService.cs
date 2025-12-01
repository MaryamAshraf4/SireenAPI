using Sireen.Application.DTOs.Payments;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResult> AddAsync(CreatePaymentDto paymentDto, int bookingId)
        {
            var payment = new Payment
            {
                BookingId = bookingId,
                PaymentDate = DateTime.UtcNow,
                AmountPaid = paymentDto.AmountPaid,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = paymentDto.PaymentMethod
            };

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Payment Created successfully.");
        }

        public async Task<IEnumerable<PaymentDto>> GetByBookingIdAsync(int bookingId)
        {
            var payments = await _unitOfWork.Payments.GetByBookingIdAsync(bookingId);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                AmountPaid = p.AmountPaid,
                PaymentStatus = p.PaymentStatus,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                RoomNumber = p.Booking.Room.RoomNumber.ToString(),
                CheckIn = p.Booking.CheckIn,
                CheckOut = p.Booking.CheckOut,
                CustomerName = p.Booking.User.FullName,
                CustomerPhoneNumber = p.Booking.User.PhoneNumber
            }).ToList();
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);

            if(payment == null)
                return null;

            return  new PaymentDto
            {
                Id = payment.Id,
                AmountPaid = payment.AmountPaid,
                PaymentStatus = payment.PaymentStatus,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                RoomNumber = payment.Booking.Room.RoomNumber.ToString(),
                CheckIn = payment.Booking.CheckIn,
                CheckOut = payment.Booking.CheckOut,
                CustomerName = payment.Booking.User.FullName,
                CustomerPhoneNumber = payment.Booking.User.PhoneNumber
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetByUserIdAsync(string userId)
        {
            var payments = await _unitOfWork.Payments.GetByUserIdAsync(userId);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                AmountPaid = p.AmountPaid,
                PaymentStatus = p.PaymentStatus,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                RoomNumber = p.Booking.Room.RoomNumber.ToString(),
                CheckIn = p.Booking.CheckIn,
                CheckOut = p.Booking.CheckOut,
                CustomerName = p.Booking.User.FullName,
                CustomerPhoneNumber = p.Booking.User.PhoneNumber
            }).ToList();
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate)
        {
            var payments = await _unitOfWork.Payments.GetPaymentsByHotelAndDateAsync(hotelId, startDate, endDate);

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                AmountPaid = p.AmountPaid,
                PaymentStatus = p.PaymentStatus,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                RoomNumber = p.Booking.Room.RoomNumber.ToString(),
                CheckIn = p.Booking.CheckIn,
                CheckOut = p.Booking.CheckOut,
                CustomerName = p.Booking.User.FullName,
                CustomerPhoneNumber = p.Booking.User.PhoneNumber
            });
        }

        public async Task<ServiceResult> UpdatePaymentStatusAsync(int paymentId, UpdatePaymentManagerDto paymentDto)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(paymentId);

            if (payment == null)
                return ServiceResult.FailureResult("Payment not found.");

            payment.PaymentStatus = paymentDto.PaymentStatus;

            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Payment status updated successfully", paymentDto.Notes);
        }
    }
}
