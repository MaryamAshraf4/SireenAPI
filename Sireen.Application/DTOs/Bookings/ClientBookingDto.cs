using Sireen.Application.DTOs.Payments;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Bookings
{
    public class ClientBookingDto
    {
        public int Id { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string HotelPhoneNumber { get; set; } = string.Empty;
        public string HotelLocation { get; set; } = string.Empty;
        public int RoomNumber { get; set; }
        public BookingStatus BookingStatus { get; set; }
        DateTime CheckIn { get; set; } = DateTime.Now;
        DateTime? CheckOut { get; set; }
        public PaymentDisplayForBookingDto? Payment { get; set; } = new PaymentDisplayForBookingDto();
        public decimal TotalAmount { get; set; }
    }
}
