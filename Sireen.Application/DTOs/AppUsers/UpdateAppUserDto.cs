using Sireen.Domain.Enums;
using Sireen.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.AppUsers
{
    public class UpdateAppUserDto
    {
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = "Full Name must contain only letters (Arabic or English).")]
        public string? FullName { get; set; }
        public IdentityType? IdentityType { get; set; }

        [MaxLength(20)]
        [IdentityNumberValidation]
        public string? IdentityNumber { get; set; } 
        public string? Nationality { get; set; }
        public DateTime? IdentityExpiryDate { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number in international format (e.g. ‪+201000000000‬).")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public bool? ChangePassword { get; set; }
    }
}
