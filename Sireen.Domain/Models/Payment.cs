using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        DateTime PaymentDate { get; set; } = DateTime.Now;

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
