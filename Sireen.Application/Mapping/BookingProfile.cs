using AutoMapper;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Bookings;
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
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingDto, Booking>()
                .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(_ => BookingStatus.Pending));

            CreateMap<Booking, ManagerBookingDto>()
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(scr => scr.Room.RoomNumber))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(scr => scr.Payment.AmountPaid))
            .ForMember(dest => dest.Payment, opt => opt.MapFrom(scr => scr.Payment))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(scr => scr.User));

            CreateMap<Booking, ClientBookingDto>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(scr => scr.Room.RoomNumber))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(scr => scr.Payment.AmountPaid))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(scr => scr.Room.Hotel.Name))
                .ForMember(dest => dest.HotelLocation, opt => opt.MapFrom(scr => scr.Room.Hotel.Location))
                .ForMember(dest => dest.HotelPhoneNumber, opt => opt.MapFrom(scr => scr.Room.Hotel.PhoneNumber))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(scr => scr.Payment));

            CreateMap<Payment, PaymentDisplayForBookingDto>();

            CreateMap<AppUser, ClientDto>();

            CreateMap<UpdateManagerBookingDto, Booking>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
