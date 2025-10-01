using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Bookings
{
    public class CancelBookingDto
    {
        [EnumDataType(typeof(BookingStatus))]
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Cancelled;
    }
}
