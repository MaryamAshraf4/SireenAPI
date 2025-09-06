using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Room
    {
        public int ID { get; set; }
        public int Capacity { get; set; }
        public int RoomNumber { get; set; }
        public bool IsDelete { get; set; }
        public double PricePerNight { get; set; }
        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        DateTime UpdatedAt { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
