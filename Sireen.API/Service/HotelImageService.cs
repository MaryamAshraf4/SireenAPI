using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sireen.API.DTOs.HotelImages;
using Sireen.API.Interfaces.IService;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Domain.Interfaces.Repository;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;

namespace Sireen.API.Service
{
    public class HotelImageService : IHotelImageService
    {
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public HotelImageService(IImageService imageService, IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<string> AddHotelImage(HotelImageUploadDto dto, string managerId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(dto.HotelId);

            if (hotel == null || hotel.IsDeleted)
                return "Hotel not found.";

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return "User not found.";

            if (!user.Hotels.Any(h => h.Id == hotel.Id))
                return "You cannot add image to this hotel.";

            string imagePath = await _imageService.UploadImageAsync(dto.Image, "HotelImages");

            var hotelImage = _mapper.Map<HotelImage>(dto);
            hotelImage.ImageUrl = imagePath;

            await _unitOfWork.HotelImages.AddAsync(hotelImage);
            await _unitOfWork.SaveChangeAsync();

            return imagePath;
        }

        public async Task<IEnumerable<HotelImageDto>> GetByHotelIdAsync(int hotelId)
        {
            var hotelImages = await _unitOfWork.HotelImages.GetByHotelIdAsync(hotelId);

            return _mapper.Map<IEnumerable<HotelImageDto>>(hotelImages);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int hotelImageId, string managerId)
        {
            var hotelImage = await _unitOfWork.HotelImages.GetByIdAsync(hotelImageId);
            if (hotelImage == null)
                return ServiceResult.FailureResult("Hotel Image not found.");

            if (hotelImage.IsDeleted)
                return ServiceResult.FailureResult("Hotel Image already deleted.");

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelImage.HotelId);

            if (hotel == null || hotel.IsDeleted)
                return ServiceResult.FailureResult("Hotel not found.");

            var user = await _userManager.FindByIdAsync(managerId);

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (!user.Hotels.Any(h => h.Id == hotel.Id))
                return ServiceResult.FailureResult("You cannot delete image from this hotel.");

            hotelImage.IsDeleted = true;
            hotelImage.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.HotelImages.Update(hotelImage);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel Image deleted successfully.");
        }
    }
}
