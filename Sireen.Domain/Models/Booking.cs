using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public BookingStatus BookingStatus { get; set; }
        DateTime CheckIn { get; set; } = DateTime.UtcNow;
        DateTime? CheckOut { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public Payment Payment { get; set; }
    }
}
