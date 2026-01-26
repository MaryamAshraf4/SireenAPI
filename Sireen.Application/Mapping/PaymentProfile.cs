using AutoMapper;
using Sireen.Application.DTOs.Payments;
using Sireen.Domain.Enums;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(_ => PaymentStatus.Pending));

            CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.RoomNumber,
                opt => opt.MapFrom(src => src.Booking.Room.RoomNumber.ToString()))
            .ForMember(dest => dest.CheckIn,
                opt => opt.MapFrom(src => src.Booking.CheckIn))
            .ForMember(dest => dest.CheckOut,
                opt => opt.MapFrom(src => src.Booking.CheckOut))
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Booking.User.FullName))
            .ForMember(dest => dest.CustomerPhoneNumber,
                opt => opt.MapFrom(src => src.Booking.User.PhoneNumber));
        }
    }
}
