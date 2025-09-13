using Sireen.Application.DTOs.Payments;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Validations
{
    public class AmountPaidRequiredForImmediateMethodsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dto = (CreatePaymentDto) validationContext.ObjectInstance;

            var requireAmount = dto.PaymentMethod is PaymentMethod.CreditCard or PaymentMethod.DebitCard or PaymentMethod.POS or PaymentMethod.EWallet;

            if (requireAmount) 
            {
                if(value is null || (decimal)value <= 0)
                {
                    return new ValidationResult("AmountPaid is required and must be > 0 for this payment method.");
                } 
            }

            return ValidationResult.Success;
        }
    }
}
