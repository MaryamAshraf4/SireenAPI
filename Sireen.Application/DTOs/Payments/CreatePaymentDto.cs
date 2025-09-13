using Sireen.Application.Validations;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Payments
{
    public class CreatePaymentDto
    {
        [AmountPaidRequiredForImmediateMethodsAttribute]
        public double AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Booking ID is required")]
        public int BookingId { get; set; }
    }
}
