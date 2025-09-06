using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Models
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
