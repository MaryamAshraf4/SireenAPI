using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Enums;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sireen.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> AddAmenityToRoomAsync(int roomId, int amenityId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.IsDelete)
                return ServiceResult.FailureResult("Room not found.");

            var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenityId);
            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found.");

            if (room.Amenities.Any(a => a.Id == amenityId))
                return ServiceResult.FailureResult("Amenity already added to this room.");

            room.Amenities.Add(amenity);
            room.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity added to room successfully.");
        }

        public async Task<ServiceResult> RemoveAmenityFromRoomAsync(int roomId, int amenityId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.IsDelete)
                return ServiceResult.FailureResult("Room not found.");

            var amenity = room.Amenities.FirstOrDefault(a => a.Id == amenityId);
            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found in this room.");

            room.Amenities.Remove(amenity);
            room.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity removed from room successfully.");
        }

        public async Task<ServiceResult> AddAsync(CreateRoomDto roomDto, int hotelId)
        {
            var room = new Room
            {
                RoomType = roomDto.RoomType,
                Capacity = roomDto.Capacity,
                RoomNumber = roomDto.RoomNumber,
                PricePerNight = roomDto.PricePerNight,   
                CreatedAt = DateTime.UtcNow,
                RoomStatus = RoomStatus.Available,
                HotelId = hotelId
            };

            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room added successfully.");
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();

            return rooms.Select(r => new RoomDto
            {
                Id = r.ID,
                Capacity = r.Capacity,
                RoomNumber= r.RoomNumber,
                PricePerNight = r.PricePerNight,
                RoomType = r.RoomType.ToString(),
                RoomStatus = r.RoomStatus.ToString(),
                RoomImages = r.RoomImages.Select(img => img.ImageUrl).ToList(),
            }).ToList();
        }

        public async Task<DisplayRoomDto?> GetByIdAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null || room.IsDelete)
                return null;

            return new DisplayRoomDto
            {
                Id = room.ID,
                Capacity = room.Capacity,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                RoomType = room.RoomType.ToString(),
                RoomStatus = room.RoomStatus.ToString(),
                RoomImages = room.RoomImages.Select(img => img.ImageUrl).ToList(),
                Amenities = room.Amenities.Select(r => new DisplayAmenityDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsFree = r.IsFree,
                    Description = r.Description
                }).ToList()

            };
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _unitOfWork.Rooms.GetRoomsByHotelIdAsync(hotelId);

            return rooms.Select(r => new RoomDto
            {
                Id = r.ID,
                Capacity = r.Capacity,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                RoomType = r.RoomType.ToString(),
                RoomStatus = r.RoomStatus.ToString(),
                RoomImages = r.RoomImages.Select(img => img.ImageUrl).ToList(),
            }).ToList();
        }

        public async Task<RoomDto> SearchAsync(int? roomNumber, int hotelId)
        {
            var room = await _unitOfWork.Rooms.SearchAsync(roomNumber, hotelId);

            return new RoomDto
            {
                Id = room.ID,
                Capacity = room.Capacity,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                RoomType = room.RoomType.ToString(),
                RoomStatus = room.RoomStatus.ToString(),
                RoomImages = room.RoomImages.Select(img => img.ImageUrl).ToList(),
            };
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null)
                return ServiceResult.FailureResult("Room not found.");

            if (room.IsDelete)
                return ServiceResult.FailureResult("Room already deleted.");

            room.IsDelete = true;
            room.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Rooms.Update(room);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room deleted successfully.");
        }

        public async Task<ServiceResult> UpdateRoomAsync(int roomId, UpdateRoomDto roomDto)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);

            if (room == null)
                return ServiceResult.FailureResult("Room not found.");

            room.Capacity = roomDto.Capacity;
            room.PricePerNight = roomDto.PricePerNight;
            room.UpdatedAt = DateTime.UtcNow;
            room.RoomType = roomDto.RoomType;
            room.RoomStatus = roomDto.RoomStatus;


            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room updated successfully");
        }
    }
}
