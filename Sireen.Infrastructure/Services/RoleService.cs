using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sireen.Application.Helpers;
using Sireen.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ServiceResult> AddRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return ServiceResult.FailureResult("Role already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if(!result.Succeeded)
                return ServiceResult.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ServiceResult.SuccessResult("Role created successfully.");
        }

        public async Task<ServiceResult> DeleteRoleAsync(string roleId)
        {
            var user = await _roleManager.FindByIdAsync(roleId);

            if (user == null)
                return ServiceResult.FailureResult("Role not found.");

            var result = await _roleManager.DeleteAsync(user);

            if(!result.Succeeded)
                return ServiceResult.FailureResult(string.Join(", ", result.Errors.Select(e => e.Description)));

            return ServiceResult.SuccessResult("Role deleted successfully.");
        }

        public async Task<ServiceResult> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return ServiceResult.SuccessResult("Roles retrieved successfully.", roles);
        }
    }
}
