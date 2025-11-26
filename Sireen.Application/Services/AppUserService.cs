using Microsoft.AspNetCore.Identity;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Domain.Interfaces.Services;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResult> RegisterUserAsync(CreateAppUserDto userDto)
        {
            var user = new AppUser
            {
                Email = userDto.Email,
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                CreatedAt = DateTime.UtcNow,
                Nationality = userDto.Nationality,
                PhoneNumber = userDto.PhoneNumber,
                IdentityType = userDto.IdentityType,
                IdentityNumber = userDto.IdentityNumber,              
                IdentityExpiryDate = userDto.IdentityExpiryDate
            };

            var result = await _userManager.CreateAsync(user,userDto.Password);

            if(!result.Succeeded)
                return ServiceResult.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Customer");
               
            return ServiceResult.SuccessResult("User registered successfully.", user.Id);
        }

        public async Task<ServiceResult> SoftDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            if (user.IsDeleted)
                return ServiceResult.FailureResult("User already deleted.");

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return ServiceResult.SuccessResult("User deleted successfully.");
        }

        public async Task<ServiceResult> UpdateUserAsync(string userId, UpdateAppUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            user.UpdatedAt = DateTime.UtcNow;

            if (user == null)
                return ServiceResult.FailureResult("User not found.");
            if(userDto.FullName != null)
                user.FullName = userDto.FullName;

            if (userDto.IdentityType != null)
                user.IdentityType = userDto.IdentityType.Value;        

            if (userDto.IdentityNumber != null)
                user.IdentityNumber = userDto.IdentityNumber;
            
            if (userDto.Nationality != null)
                user.Nationality = userDto.Nationality;
           
            if (userDto.IdentityExpiryDate != null)
                user.IdentityExpiryDate = userDto.IdentityExpiryDate;
            
            if (userDto.PhoneNumber != null)
                user.PhoneNumber = userDto.PhoneNumber;
           
            if (userDto.Email != null)
                user.Email = userDto.Email;


            await _userManager.UpdateAsync(user);

            return ServiceResult.SuccessResult("User updated successfully");
        }

        public async Task<AppUserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            return new AppUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentityType = user.IdentityType,
                IdentityNumber = user.IdentityNumber,
                Nationality = user.Nationality,
                IdentityExpiryDate = user.IdentityExpiryDate,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
