using Sireen.Application.DTOs.Payments;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Bookings
{
    public class CreateBookingDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOut { get; set; }
    }
}
