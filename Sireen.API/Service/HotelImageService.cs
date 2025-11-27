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

        public HotelImageService(IImageService imageService, IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> AddHotelImage(HotelImageUploadDto dto)
        {
            string imagePath = await _imageService.UploadImageAsync(dto.Image, "HotelImages");

            var hotelImage = new HotelImage
            {
                HotelId = dto.HotelId,
                ImageUrl = imagePath
            };

            await _unitOfWork.HotelImages.AddAsync(hotelImage);
            await _unitOfWork.SaveChangeAsync();

            return imagePath;
        }

        public async Task<IEnumerable<HotelImageDto>> GetByHotelIdAsync(int hotelId)
        {
            var hotelImages = await _unitOfWork.HotelImages.GetByHotelIdAsync(hotelId);

            return hotelImages.Select(hi => new HotelImageDto
            {
                Id = hi.Id,
                ImageUrl = hi.ImageUrl,
                CreatedAt = hi.CreatedAt,
               UpdatedAt = hi.UpdatedAt    
            }).ToList();
        }

        public async Task<ServiceResult> SoftDeleteAsync(int hotelImageId)
        {
            var hotelImage = await _unitOfWork.HotelImages.GetByIdAsync(hotelImageId);
            if (hotelImage == null)
                return ServiceResult.FailureResult("Hotel Image not found.");

            if (hotelImage.IsDeleted)
                return ServiceResult.FailureResult("Hotel Image already deleted.");

            hotelImage.IsDeleted = true;
            hotelImage.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.HotelImages.Update(hotelImage);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel Image deleted successfully.");
        }
    }
}
