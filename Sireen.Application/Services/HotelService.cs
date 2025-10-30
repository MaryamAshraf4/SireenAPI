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

namespace Sireen.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> AddAsync(CreateHotelDto hotelDto, string managerId)
        {
            var hotel = new Hotel
            {
                IsDeleted = false,
                Name = hotelDto.Name,
                Email = hotelDto.Email,
                Location = hotelDto.Location,
                CreatedAt = DateTime.UtcNow,
                PhoneNumber = hotelDto.PhoneNumber,              
                Description = hotelDto.Description,
                ManagerId = managerId
            };

            await _unitOfWork.Hotels.AddAsync(hotel);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel added successfully.");
        }

        public async Task<IEnumerable<HotelDto>> GetAllAsync()
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();

            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Email = h.Email,
                Location = h.Location,
                PhoneNumber = h.PhoneNumber,
                Description = h.Description,
                HotelImages = h.HotelImages.Select(img => img.ImageUrl).ToList(),
            }).ToList();
        }

        public async Task<DisplayHotelDto?> GetByIdAsync(int id)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
            if (hotel == null || hotel.IsDeleted)
                return null;

            return new DisplayHotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Email = hotel.Email,
                Location = hotel.Location,
                PhoneNumber = hotel.PhoneNumber,
                Description = hotel.Description,
                HotelImages = hotel.HotelImages.Select(img => img.ImageUrl).ToList(),
                Rooms = hotel.Rooms.Select(r => new RoomDto
                {
                    Id = r.ID,
                    Capacity = r.Capacity,
                    RoomNumber = r.RoomNumber,
                    PricePerNight = r.PricePerNight,
                    RoomType = r.RoomType.ToString(),
                    RoomStatus = r.RoomStatus.ToString(),
                    RoomImages = r.RoomImages.Select(img => img.ImageUrl).ToList()
                }).ToList()

            };
        }

        public async Task<IEnumerable<HotelDto>> GetHotelsByManagerIdAsync(string managerId)
        {
            var hotels = await _unitOfWork.Hotels.GetHotelsByManagerIdAsync(managerId);

            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Email = h.Email,
                Location = h.Location,
                PhoneNumber = h.PhoneNumber,
                Description = h.Description,
                HotelImages = h.HotelImages.Select(img => img.ImageUrl).ToList(),
            }).ToList();
        }

        public async Task<IEnumerable<HotelDto>> SearchAsync(string? name, string? location)
        {
            var hotels = await _unitOfWork.Hotels.SearchAsync(name, location);

            return hotels.Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Email = h.Email,
                Location = h.Location,
                PhoneNumber = h.PhoneNumber,
                Description = h.Description,
                HotelImages = h.HotelImages.Select(img => img.ImageUrl).ToList(),
            }).ToList();
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

            hotel.Name = hotelDto.Name;
            hotel.Email = hotelDto.Email;
            hotel.UpdatedAt = DateTime.UtcNow;
            hotel.Location = hotelDto.Location;
            hotel.PhoneNumber = hotelDto.PhoneNumber;
            if(hotelDto.Description != null)
                hotel.Description = hotelDto.Description;

            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.SaveChangeAsync();

            return ServiceResult.SuccessResult("Hotel updated successfully");
        }
    }
}
