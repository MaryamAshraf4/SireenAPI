using Sireen.Application.DTOs.Bookings;
using Sireen.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.AppUsers
{
    public class AppUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IdentityType IdentityType { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime? IdentityExpiryDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<string> HotelNames { get; set; } = new List<string>();
        public List<ClientBookingDto> ClientBookings { get; set; } = new List<ClientBookingDto>();
    }
}
