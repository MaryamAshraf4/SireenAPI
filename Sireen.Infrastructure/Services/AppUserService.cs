using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.Services;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Sireen.Infrastructure.Configurations;

namespace Sireen.Infrastructure.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwt;
        public AppUserService(UserManager<AppUser> userManager, IMapper mapper, IOptions<JwtSettings> jwt)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<ServiceResult> RegisterUserAsync(CreateAppUserDto userDto)
        {
            if(await _userManager.FindByEmailAsync(userDto.Email) is not null)
                return ServiceResult.FailureResult("Email is already registered!");

            if(await _userManager.FindByNameAsync(userDto.UserName) is not null)
                return ServiceResult.FailureResult("User Name is already registered!");

            var user = _mapper.Map<AppUser>(userDto);              

            var result = await _userManager.CreateAsync(user,userDto.Password);

            if(!result.Succeeded)
                return ServiceResult.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Customer");

            var jwtSecurityToken = await CreateJwtToken(user); 
               
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

        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.TokenExpirationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
