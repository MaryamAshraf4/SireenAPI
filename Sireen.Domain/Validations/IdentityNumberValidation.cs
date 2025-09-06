using Sireen.Domain.Enums;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sireen.Domain.Validations
{
    public class IdentityNumberValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
        {
            var user = (AppUser) validationContext.ObjectInstance;
            var identityNumber = value as string;
            if( user.IdentityType == IdentityType.NationalId) 
            {
                if (string.IsNullOrEmpty(identityNumber) || !Regex.IsMatch(identityNumber, @"^\d{14}$"))
                    return new ValidationResult("National ID must be exactly 14 digits.");
            }
            else if (user.IdentityType == IdentityType.Passport)
            {
                if (string.IsNullOrEmpty(identityNumber) || !Regex.IsMatch(identityNumber, @"^[A-Z0-9]{6,9}$"))
                    return new ValidationResult("Passport must be 6–9 characters.");
            }
            return ValidationResult.Success!;
        }
    }
}
