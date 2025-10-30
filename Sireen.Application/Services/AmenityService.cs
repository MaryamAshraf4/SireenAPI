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
        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> AddAsync(CreateAmenityDto amenityDto)
        {
            var amenity = new Amenity
            {
                IsFree = true,
                Name = amenityDto.Name,
                Description = amenityDto.Description
            };

            await _unitOfWork.Amenities.AddAsync(amenity);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity added successfully.");
        }

        public async Task<IEnumerable<AmenityDto>> GetAllAsync()
        {
            var amenities = await _unitOfWork.Amenities.GetAllAsync();

            return amenities.Select( a => new AmenityDto
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
        }

        public async Task<DisplayAmenityDto?> GetByIdAsync(int id)
        {
            var amenity = await _unitOfWork.Amenities.GetByIdAsync(id);

            if (amenity == null)
                return null;

            return  new DisplayAmenityDto
            {
                Id = amenity.Id,
                IsFree = true,
                Name = amenity.Name,
                Description = amenity.Description
            };
        }

        public async Task<IEnumerable<AmenityDto>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var amenities = await _unitOfWork.Amenities.GetByIdsAsync(ids);

            return amenities.Select(a => new AmenityDto
            {
                Id = a.Id,
                Name = a.Name
            }).ToList();
        }

        public async Task<ServiceResult> UpdateAmenityAsync(int amenitylId, UpdateAmenityDto amenityDto)
        {
            var amenity = await _unitOfWork.Amenities.GetByIdAsync(amenitylId);

            if (amenity == null)
                return ServiceResult.FailureResult("Amenity not found.");

            amenity.Name = amenityDto.Name;
            amenity.IsFree = amenityDto.IsFree;

            if(amenityDto.Description != null)
                amenity.Description = amenityDto.Description;

            _unitOfWork.Amenities.Update(amenity);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Amenity updated successfully");
        }
    }
}
