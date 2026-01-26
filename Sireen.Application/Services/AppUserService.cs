using AutoMapper;
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
        private readonly IMapper _mapper;
        public AppUserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ServiceResult> RegisterUserAsync(CreateAppUserDto userDto)
        {
            var user = _mapper.Map<AppUser>(userDto);              

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

            if (user == null)
                return ServiceResult.FailureResult("User not found.");

            _mapper.Map(userDto, user);

            user.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return ServiceResult.SuccessResult("User updated successfully");
        }

        public async Task<AppUserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.IsDeleted)
                return null;

            return _mapper.Map<AppUserDto>(user);                
        }
    }
}
