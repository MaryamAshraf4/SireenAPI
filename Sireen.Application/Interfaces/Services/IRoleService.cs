using Sireen.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ServiceResult> AddRoleAsync(string roleName);
        Task<ServiceResult> GetAllRolesAsync();
        Task<ServiceResult> DeleteRoleAsync(string roleId);
    }
}
