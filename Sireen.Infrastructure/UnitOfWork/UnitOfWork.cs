using Sireen.Domain.Interfaces.Repository;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Infrastructure.Persistence;
using Sireen.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IHotelRepository? _hotels;
        private IRoomRepository? _rooms;
        private IBookingRepository? _bookings;
        private IAmenityRepository? _amenities;
        private IPaymentRepository? _payments;
        private IRatingRepository? _ratings;
        private IRoomImageRepository? _roomImages;
        private IHotelImageRepository? _hotelImages;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IHotelRepository Hotels 
        {
            get
            {
                if(_hotels == null)
                {
                    _hotels = new HotelRepository(_context);
                }
                return _hotels;
            }
        }

        public IRoomRepository Rooms
        {
            get
            {
                if (_rooms == null)
                {
                    _rooms = new RoomRepository(_context);
                }
                return _rooms;
            }
        }

        public IBookingRepository Bookings
        {
            get
            {
                if (_bookings == null)
                {
                    _bookings = new BookingRepository(_context);
                }
                return _bookings;
            }
        }

        public IAmenityRepository Amenities
        {
            get
            {
                if (_amenities == null)
                {
                    _amenities = new AmenityRepository(_context);
                }
                return _amenities;
            }
        }

        public IPaymentRepository Payments
        {
            get
            {
                if (_payments == null)
                {
                    _payments = new PaymentRepository(_context);
                }
                return _payments;
            }
        }

        public IRatingRepository Ratings
        {
            get
            {
                if (_ratings == null)
                {
                    _ratings = new RatingRepository(_context);
                }
                return _ratings;
            }
        }

        public IRoomImageRepository RoomImages
        {
            get
            {
                if (_roomImages == null)
                {
                    _roomImages = new RoomImageRepository(_context);
                }
                return _roomImages;
            }
        }

        public IHotelImageRepository HotelImages
        {
            get
            {
                if (_hotelImages == null)
                {
                    _hotelImages = new HotelImageRepository(_context);
                }
                return _hotelImages;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
