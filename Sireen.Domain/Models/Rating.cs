using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
