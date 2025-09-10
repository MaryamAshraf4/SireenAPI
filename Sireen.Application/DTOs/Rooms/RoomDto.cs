using Sireen.Application.DTOs.Amenities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Rooms
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int RoomNumber { get; set; }
        public double PricePerNight { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string RoomStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<string> RoomImages { get; set; } = new List<string>();
        public List<AmenityDto> Amenities { get; set; } = new List<AmenityDto>();
    }

}
