using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.DTOs.Amenities
{
    public class CreateAmenityDto
    {
        [Required(ErrorMessage = "Amenity name is required.")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsFree { get; set; } = true;
    }
}
