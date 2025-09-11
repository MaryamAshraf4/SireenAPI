using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Ratings
{
    public class CreateRatingDto
    {
        [Required(ErrorMessage = "Score is required.")]
        [Range(1, 5, ErrorMessage = "Score must be between 1 and 5.")]
        public int Score { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public int HotelId { get; set; }
    }
}
