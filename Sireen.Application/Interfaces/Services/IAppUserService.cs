using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Rooms;
using Sireen.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Domain.Interfaces.Services
{
    public interface IAppUserService
    {
        Task<ServiceResult> RegisterUserAsync(CreateAppUserDto userDto);
        Task<ServiceResult> UpdateUserAsync(string userId, UpdateAppUserDto userDto);
        Task<ServiceResult> SoftDeleteAsync(string id);
        Task<AppUserDto?> GetByIdAsync(string id);
        Task<AuthDto> GetTokenAsync(TokenRequestDto tokenRequestDto);
    }
}
