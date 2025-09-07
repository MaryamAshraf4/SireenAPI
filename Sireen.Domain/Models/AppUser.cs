using Microsoft.AspNetCore.Identity;
using Sireen.Domain.Enums;
using Sireen.Domain.Validations;
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
        [Required(ErrorMessage = "Full Name is required.")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = "Full Name must contain only letters (Arabic or English).")]
        public string FullName { get; set; } = string.Empty;
        public IdentityType IdentityType { get; set; }

        [Required(ErrorMessage = "Identity Number is required.")]
        [MaxLength(20)]
        [IdentityNumberValidation]
        public string IdentityNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } = string.Empty;
        public DateTime? IdentityExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
