using Sireen.API.DTOs.RoomImages;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.DTOs.RoomImages;

namespace Sireen.API.Interfaces.IService
{
    public interface IRoomImageService
    {
        Task<string> AddRoomImage(RoomImageUploadDto dto);
        Task<IEnumerable<RoomImageDto>> GetByRoomIdAsync(int roomId);
    }
}
