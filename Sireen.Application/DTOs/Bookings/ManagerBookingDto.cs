using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Payments;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Bookings
{
    public class ManagerBookingDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOut { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentDisplayForBookingDto? Payment { get; set; } = new PaymentDisplayForBookingDto();
        public ClientDto Client { get; set; } = new ClientDto();
    }
}
