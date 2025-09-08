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

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number in international format (e.g. ‪+201000000000‬).")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
        public bool ChangePassword { get; set; }
    }
}
