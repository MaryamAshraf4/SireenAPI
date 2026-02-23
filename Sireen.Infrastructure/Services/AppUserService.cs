using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using Sireen.Domain.Interfaces.Services;
using Sireen.Domain.Models;
using Sireen.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return ServiceResult.SuccessResult($"Please confirm your email with code that you have received {token}.");

        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.IsDeleted)
                return null;

            return user;
        }
        public async Task<bool> ConfirmEmailAsync(AppUser user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }

        public async Task<AuthDto> GetTokenAsync(TokenRequestDto tokenRequestDto)
        {
            var authDto = new AuthDto();

            var user = _userManager.Users.FirstOrDefault(u => u.Email == tokenRequestDto.Email && !u.IsDeleted);

            if (!await _userManager.IsEmailConfirmedAsync(user)) {
                authDto.Message = "Email is not Confirmed.";
                return authDto;
            }

            if (user is null || !_userManager.CheckPasswordAsync(user, tokenRequestDto.Password).Result)
            {
                authDto.Message = "Invalid email or password.";
                return authDto;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            authDto.IsAuthenticated = true;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDto.ExpiresOn = jwtSecurityToken.ValidTo;
            authDto.Email = user.Email;
            authDto.Username = user.UserName;
            authDto.Roles = roles.ToList();

            if(user.RefreshTokens.Any(r => r.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.First(r => r.IsActive);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

           return authDto;
        }

        public async Task<AuthDto> RefreshTokenAsync(string token)
        {
            var authDto = new AuthDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authDto.Message = "Invalid token";
                return authDto;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authDto.Message = "Inactive token";
                return authDto;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authDto.IsAuthenticated = true;
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authDto.Email = user.Email;
            authDto.Username = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authDto.Roles = roles.ToList();
            authDto.RefreshToken = newRefreshToken.Token;
            authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authDto;
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

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
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
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
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

        private RefreshToken GenerateRefreshToken() 
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    ExpiresOn = DateTime.Now.AddDays(10),
                    CreatedOn = DateTime.Now
                };
            }
        }
    }
}
