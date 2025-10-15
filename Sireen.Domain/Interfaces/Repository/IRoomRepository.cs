using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task SoftDeleteAsync(int id);
        Task<Room> SearchAsync(int? roomNumber);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
