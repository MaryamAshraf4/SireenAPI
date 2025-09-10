using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Rooms
{
    public class UpdateRoomDto
    {
        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Price per night is required.")]
        public double PricePerNight { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public RoomType RoomType { get; set; }

        [Required(ErrorMessage = "Room status is required.")]
        public RoomStatus RoomStatus { get; set; }
    }
}
