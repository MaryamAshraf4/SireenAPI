using Sireen.Application.DTOs.AppUsers;
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
        DateTime CheckIn { get; set; } = DateTime.Now;
        DateTime? CheckOut { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal? Amount { get; set; }

        public ClientDto Client { get; set; } = new ClientDto();
    }
}
