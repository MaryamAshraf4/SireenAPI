using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        Task SoftDeleteAsync(int id);
        Task<Room?> GetByIdAsync(int id);
        Task<Room> SearchAsync(int? roomNumber);
        Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId);
    }
}
