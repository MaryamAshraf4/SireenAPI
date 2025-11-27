using Sireen.API.DTOs.HotelImages;
using Sireen.API.DTOs.RoomImages;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.Helpers;

namespace Sireen.API.Interfaces.IService
{
    public interface IHotelImageService
    {
        Task<string> AddHotelImage(HotelImageUploadDto dto);
        Task<IEnumerable<HotelImageDto>> GetByHotelIdAsync(int hotelId);
        Task<ServiceResult> SoftDeleteAsync(int hotelImageId);
    }
}
