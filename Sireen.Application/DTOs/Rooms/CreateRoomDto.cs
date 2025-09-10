using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Rooms
{
    public class CreateRoomDto
    {
        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Price per night is required.")]
        public double PricePerNight { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public RoomType RoomType { get; set; }
    }
}
