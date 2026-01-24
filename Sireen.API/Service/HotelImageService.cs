using AutoMapper;
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

        public HotelImageService(IImageService imageService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<string> AddHotelImage(HotelImageUploadDto dto)
        {
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
