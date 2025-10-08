using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IHotelImageRepository : IGenericRepository<HotelImage>
    {
        Task<IEnumerable<HotelImage>> GetByHotelIdAsync(int hotelId);
    }
}
