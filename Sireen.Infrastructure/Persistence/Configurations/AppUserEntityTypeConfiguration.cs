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
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FullName).HasMaxLength(200).IsRequired();
            builder.Property(u => u.IdentityNumber).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Nationality).HasMaxLength(100).IsRequired();
            builder.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.HasQueryFilter(u => !u.IsDeleted);
            builder.Property(u => u.IdentityType).HasConversion<string>().IsRequired();
        }
    }
}
