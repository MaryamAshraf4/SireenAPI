using Sireen.Application.DTOs.Hotels;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IRoomService
    {        
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<DisplayRoomDto?> GetByIdAsync(int id);
        Task<ServiceResult> SoftDeleteAsync(int id);
        Task<RoomDto> SearchAsync(int? roomNumber, int hotelId);
        Task<IEnumerable<RoomDto>> GetRoomsByHotelIdAsync(int hotelId);
        Task<ServiceResult> AddAsync(CreateRoomDto roomDto, int hotelId);
        Task<ServiceResult> UpdateRoomAsync(int roomId, UpdateRoomDto roomDto);
        Task<ServiceResult> AddAmenityToRoomAsync(int roomId, int amenityId);
        Task<ServiceResult> RemoveAmenityFromRoomAsync(int roomId, int amenityId);
    }
}
