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
    public class RoomEntityTypeConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(r => r.Capacity).IsRequired();
            builder.Property(r => r.RoomNumber).IsRequired();
            builder.HasQueryFilter(r => !r.IsDelete);
            builder.Property(r => r.PricePerNight).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(r => r.RoomType).HasConversion<string>().IsRequired();
            builder.Property(r => r.RoomStatus).HasConversion<string>().IsRequired();

            builder.HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
