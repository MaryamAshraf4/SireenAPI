using Microsoft.AspNetCore.Identity;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class AppUser : IdentityUser
    {    
        public string FullName { get; set; } = string.Empty;
        public IdentityType IdentityType { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime? IdentityExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
