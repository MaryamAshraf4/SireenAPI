using AutoMapper;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.UnitOfWork;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult> AddAsync(CreateHotelDto hotelDto, string managerId)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);

            hotel.ManagerId = managerId;

            await _unitOfWork.Hotels.AddAsync(hotel);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel added successfully.");
        }

        public async Task<IEnumerable<HotelDto>> GetAllAsync()
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();

            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<DisplayHotelDto?> GetByIdAsync(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
            if (hotel == null || hotel.IsDeleted)
                return null;

            return _mapper.Map<DisplayHotelDto>(hotel);
        }

        public async Task<IEnumerable<HotelDto>> GetHotelsByManagerIdAsync(string managerId)
        {
            var hotels = await _unitOfWork.Hotels.GetHotelsByManagerIdAsync(managerId);

            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<IEnumerable<HotelDto>> SearchAsync(string? name, string? location)
        {
            var hotels = await _unitOfWork.Hotels.SearchAsync(name, location);

            return _mapper.Map<IEnumerable<HotelDto>>(hotels);
        }

        public async Task<ServiceResult> SoftDeleteAsync(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
            if (hotel == null)
                return ServiceResult.FailureResult("Hotel not found.");

            if (hotel.IsDeleted)
                return ServiceResult.FailureResult("Hotel already deleted.");

            hotel.IsDeleted = true;
            hotel.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Hotels.Update(hotel);

            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel deleted successfully.");
        }

        public async Task<ServiceResult> UpdateHotelAsync(int hotelId, UpdateHotelDto hotelDto)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);

            if (hotel == null)
                return ServiceResult.FailureResult("Hotel not found.");

            _mapper.Map(hotelDto, hotel);
            hotel.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel updated successfully");
        }
    }
}
