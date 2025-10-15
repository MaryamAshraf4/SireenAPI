using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces.Repository
{
    public interface IRoomImageRepository : IGenericRepository<RoomImage>
    {
        Task<IEnumerable<RoomImage>> GetByRoomIdAsync(int roomId);
    }
}
