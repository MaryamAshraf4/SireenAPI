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
    public class HotelEntityTypeConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.Property(h => h.Name).HasMaxLength(100).IsRequired();
            builder.Property(h => h.Email).HasMaxLength(256).IsRequired();
            builder.Property(h => h.Location).HasMaxLength(200).IsRequired();
            builder.Property(h => h.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(h => h.Description).HasMaxLength(1000).IsRequired();
            builder.HasQueryFilter(h => !h.IsDeleted);
        }
    }
}
