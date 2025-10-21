using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces.Repository
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<IEnumerable<Hotel>> GetHotelsByManagerIdAsync(string managerId);
        Task<IEnumerable<Hotel>> SearchAsync(string? name, string? location);
    }
}
