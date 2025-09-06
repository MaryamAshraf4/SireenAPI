using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Hotel
    {
        int Id { get; set; }
        string Name { get; set; } = string.Empty;
        string Location { get; set; } = string.Empty;
        string PhoneNumber { get; set; } = string.Empty;
        string Email { get; set; } = string.Empty;
        string Description { get; set; } = string.Empty;
        bool IsDeleted { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        DateTime UpdatedAt { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; } = new List<HotelImage>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public AppUser Manager { get; set; }
    }
}
