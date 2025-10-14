using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
        IAmenityRepository Amenities { get; }
        IPaymentRepository Payments { get; }
        IRatingRepository Ratings { get; }
        IRoomImageRepository RoomImages { get; }
        IHotelImageRepository HotelImages { get; }
        Task SaveChangeAsync();
    }
}
