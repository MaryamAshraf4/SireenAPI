using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

namespace Sireen.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResult> AddAmenityToRoomAsync(int roomId, int amenityId, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.IsDelete)
                return ServiceResult.FailureResult("Room not found.");

            var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenityId);
            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return ServiceResult.FailureResult("You cannot add amenity to this room.");

            if (room.Amenities.Any(a => a.Id == amenityId))
                return ServiceResult.FailureResult("Amenity already added to this room.");

            room.Amenities.Add(amenity);
            room.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity added to room successfully.");
        }

        public async Task<ServiceResult> RemoveAmenityFromRoomAsync(int roomId, int amenityId, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null || room.IsDelete)
                return ServiceResult.FailureResult("Room not found.");

            var amenity = room.Amenities.FirstOrDefault(a => a.Id == amenityId);
            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found in this room.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return ServiceResult.FailureResult("You cannot delete amenity from this room.");

            room.Amenities.Remove(amenity);
            room.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity removed from room successfully.");
        }

        public async Task<ServiceResult> AddAsync(CreateRoomDto roomDto, int hotelId, string managerId)
        {
            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == hotelId))
                return ServiceResult.FailureResult("You cannot add room to this hotel.");

            var room = _mapper.Map<Room>(roomDto);

            room.HotelId = hotelId;     

            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room added successfully.");
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();

            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<DisplayRoomDto?> GetByIdAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null || room.IsDelete)
                return null;

            return _mapper.Map<DisplayRoomDto>(room);
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId)
        {
            var rooms = await _unitOfWork.Rooms.GetRoomsByHotelIdAsync(hotelId);

            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto> SearchAsync(int? roomNumber, int hotelId)
        {
            var room = await _unitOfWork.Rooms.SearchAsync(roomNumber, hotelId);

            return _mapper.Map<RoomDto>(room);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null)
                return ServiceResult.FailureResult("Room not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return ServiceResult.FailureResult("You cannot delete room from this hotel.");

            if (room.IsDelete)
                return ServiceResult.FailureResult("Room already deleted.");

            room.IsDelete = true;
            room.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Rooms.Update(room);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room deleted successfully.");
        }

        public async Task<ServiceResult> UpdateRoomAsync(int roomId, UpdateRoomDto roomDto, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);

            if (room == null)
                return ServiceResult.FailureResult("Room not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return ServiceResult.FailureResult("You cannot edit this room.");

            _mapper.Map(roomDto, room);

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room updated successfully");
        }
    }
}
