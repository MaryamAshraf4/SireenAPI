using Sireen.Application.DTOs.Hotels;
using Sireen.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IHotelService
    {
        Task<ServiceResult> AddAsync(CreateHotelDto hotelDto);
        Task<IEnumerable<HotelDto>> GetAllAsync();
        Task<DisplayHotelDto?> GetByIdAsync(int id);
        Task<IEnumerable<HotelDto>> SearchAsync(string? name, string? location);
        Task<IEnumerable<HotelDto>> GetHotelsByManagerIdAsync(string managerId);
        Task<ServiceResult> SoftDeleteAsync(int id);
        Task<ServiceResult> UpdateHotelAsync(int hotelId, UpdateHotelDto hotelDto);
    }
}
