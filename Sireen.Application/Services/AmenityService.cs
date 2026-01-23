using AutoMapper;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> AddAsync(CreateAmenityDto amenityDto)
        {
            var amenity = _mapper.Map<Amenity>(amenityDto);

            await _unitOfWork.Amenities.AddAsync(amenity);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity added successfully.");
        }

        public async Task<IEnumerable<AmenityDto>> GetAllAsync()
        {
            var amenities = await _unitOfWork.Amenities.GetAllAsync();

            return _mapper.Map<IEnumerable<AmenityDto>>(amenities);
        }

        public async Task<DisplayAmenityDto?> GetByIdAsync(int id)
        {
            var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);

            if (amenity == null)
                return null;

            return _mapper.Map<DisplayAmenityDto>(amenity);
        }

        public async Task<ServiceResult> UpdateAmenityAsync(int amenitylId, UpdateAmenityDto amenityDto)
        {
            var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenitylId);

            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found.");

            _mapper.Map(amenityDto, amenity);

            _unitOfWork.Amenities.Update(amenity);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity updated successfully");
        }
    }
}
