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
        string Name { get; set; }
        string Location { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        string Description { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        DateTime UpdatedAt { get; set; }
        public ICollection<HotelImage> HotelImages { get; set; } = new List<HotelImage>();

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
        public AppUser Manager { get; set; }
    }
}
