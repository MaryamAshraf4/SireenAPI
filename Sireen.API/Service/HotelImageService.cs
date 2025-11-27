using Sireen.API.DTOs.HotelImages;
using Sireen.API.Interfaces.IService;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Application.DTOs.Rooms;
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
    }
}
