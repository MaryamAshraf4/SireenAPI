using AutoMapper;
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

namespace Sireen.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResult> AddAsync(CreatePaymentDto paymentDto, int bookingId)
        {
            var payment = _mapper.Map<Payment>(paymentDto);

            payment.BookingId = bookingId;

            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Payment Created successfully.");
        }

        public async Task<IEnumerable<PaymentDto>> GetByBookingIdAsync(int bookingId)
        {
            var payments = await _unitOfWork.Payments.GetByBookingIdAsync(bookingId);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);                
        }

        public async Task<PaymentDto?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);

            if(payment == null)
                return null;

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetByUserIdAsync(string userId)
        {
            var payments = await _unitOfWork.Payments.GetByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByHotelAndDateAsync(int hotelId, DateTime? startDate, DateTime? endDate)
        {
            var payments = await _unitOfWork.Payments.GetPaymentsByHotelAndDateAsync(hotelId, startDate, endDate);

            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
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
