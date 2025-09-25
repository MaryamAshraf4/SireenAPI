using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Persistence.Configurations
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.AmountPaid).HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentStatus).HasConversion<string>().IsRequired();

            builder.Property(p => p.PaymentMethod).HasConversion<string>().IsRequired();

            builder.HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);
        }
    }
}
