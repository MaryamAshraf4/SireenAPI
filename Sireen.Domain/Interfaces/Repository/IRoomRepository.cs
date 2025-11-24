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
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> SearchAsync(int? roomNumber, int hotelId);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
