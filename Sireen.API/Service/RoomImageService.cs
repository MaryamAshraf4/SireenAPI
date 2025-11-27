using Sireen.API.DTOs.RoomImages;
using Sireen.API.Interfaces.IService;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.DTOs.RoomImages;
using Sireen.Domain.Interfaces.Repository;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;

namespace Sireen.API.Service
{
    public class RoomImageService : IRoomImageService
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        public RoomImageService(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> AddRoomImage(RoomImageUploadDto dto)
        {
            string imagePath = await _imageService.UploadImageAsync(dto.Image, "RoomImages");

            var roomImage = new RoomImage
            {
                RoomId = dto.RoomId,
                ImageUrl = imagePath
            };

            await _unitOfWork.RoomImages.AddAsync(roomImage);
            await _unitOfWork.SaveChangeAsync();

            return imagePath;
        }

        public async Task<IEnumerable<RoomImageDto>> GetByRoomIdAsync(int roomId)
        {
            var roomImages = await _unitOfWork.RoomImages.GetByRoomIdAsync(roomId);

            return roomImages.Select(ri => new RoomImageDto
            {
                Id = ri.Id,
                ImageUrl = ri.ImageUrl,
                CreatedAt = ri.CreatedAt,
                UpdatedAt = ri.UpdatedAt
            }).ToList();
        }
    }
}
