using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.Helpers;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IAmenityService
    {
        Task<ServiceResult> AddAsync(CreateAmenityDto amenityDto);
        Task<IEnumerable<AmenityDto>> GetAllAsync();
        Task<DisplayAmenityDto?> GetByIdAsync(int id);
        Task<ServiceResult> UpdateAmenityAsync(int amenitylId, UpdateAmenityDto amenityDto);
    }
}
