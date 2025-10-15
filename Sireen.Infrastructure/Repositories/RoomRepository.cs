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
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context) { }

        public override async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.Include(r => r.RoomImages)
                .Include(r => r.Amenities).FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
        }

        public async Task<Room> SearchAsync(int? roomNumber)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }

        public async Task SoftDeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if(room != null)
            {
                room.IsDelete = true;
            }
        }
    }
}
