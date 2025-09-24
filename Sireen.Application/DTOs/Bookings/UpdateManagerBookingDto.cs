using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Bookings
{
    public class UpdateManagerBookingDto
    {
        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus BookingStatus { get; set; } 

        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public int? RoomId { get; set; }
    }
}
