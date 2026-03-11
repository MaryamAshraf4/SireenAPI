using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        public RoomImageService(IImageService imageService, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<string> AddRoomImage(RoomImageUploadDto dto, string managerId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(dto.RoomId);

            if (room == null || room.IsDelete)
                return "Room not found.";

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return "User not found.";

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return "You cannot add image to this room.";

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

        public async Task<ServiceResult> SoftDeleteAsync(int roomImageId, string managerId)
        {            
            var roomImage = await _unitOfWork.RoomImages.GetByIdAsync(roomImageId);
            if (roomImage == null)
                return ServiceResult.FailureResult("Room Image not found.");

            if (roomImage.IsDeleted)
                return ServiceResult.FailureResult("Room Image already deleted.");

            var room = await _unitOfWork.Rooms.GetByIdAsync(roomImage.RoomId);

            if (room == null || room.IsDelete)
                return ServiceResult.FailureResult("Room not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == room.HotelId))
                return ServiceResult.FailureResult("You cannot delete image from this room.");

            roomImage.IsDeleted = true;
            roomImage.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.RoomImages.Update(roomImage);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Room Image deleted successfully.");
        }
    }
}
