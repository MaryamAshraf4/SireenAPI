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
    public class AmenityEntityTypeConfiguration : IEntityTypeConfiguration<Amenity>
    {
        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Description).HasMaxLength(500);
        }
    }
}
