using Microsoft.EntityFrameworkCore;
using Sireen.Domain.Interfaces.Repository;
using Sireen.Domain.Models;
using Sireen.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Repositories
{
    public class RoomImageRepository : GenericRepository<RoomImage>, IRoomImageRepository
    {
        public RoomImageRepository(AppDbContext context) : base(context){}

        public async Task<IEnumerable<RoomImage>> GetByRoomIdAsync(int roomId)
        {
            return await _context.RoomImages.Where(ri => ri.RoomId == roomId).ToListAsync();
        }

    }
}
