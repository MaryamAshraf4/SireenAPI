using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IAmenityRepository : IGenericRepository<Amenity>
    {  
        Task<IEnumerable<Amenity>> GetAllAsync();
        Task<IEnumerable<Amenity>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
