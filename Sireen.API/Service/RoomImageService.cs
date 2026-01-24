using AutoMapper;
using Sireen.API.DTOs.RoomImages;
using Sireen.API.Interfaces.IService;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.DTOs.RoomImages;
using Sireen.Application.Helpers;
using Sireen.Domain.Interfaces.Repository;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;

namespace Sireen.API.Service
{
    public class RoomImageService : IRoomImageService
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomImageService(IImageService imageService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<string> AddRoomImage(RoomImageUploadDto dto)
        {
            string imagePath = await _imageService.UploadImageAsync(dto.Image, "RoomImages");

            var roomImage = _mapper.Map<RoomImage>(dto);
            roomImage.ImageUrl = imagePath;

            await _unitOfWork.RoomImages.AddAsync(roomImage);
            await _unitOfWork.SaveChangeAsync();

            return imagePath;
        }

        public async Task<IEnumerable<RoomImageDto>> GetByRoomIdAsync(int roomId)
        {
            var roomImages = await _unitOfWork.RoomImages.GetByRoomIdAsync(roomId);

            return _mapper.Map<IEnumerable<RoomImageDto>>(roomImages);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int roomImageId)
        {
            var roomImage = await _unitOfWork.RoomImages.GetByIdAsync(roomImageId);
            if (roomImage == null)
                return ServiceResult.FailureResult("Room Image not found.");

            if (roomImage.IsDeleted)
                return ServiceResult.FailureResult("Room Image already deleted.");

            roomImage.IsDeleted = true;
            roomImage.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.RoomImages.Update(roomImage);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room Image deleted successfully.");
        }
    }
}
