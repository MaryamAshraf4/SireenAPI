using Microsoft.AspNetCore.Identity;
using Sireen.Application.DTOs.AppUsers;
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
    }
}
