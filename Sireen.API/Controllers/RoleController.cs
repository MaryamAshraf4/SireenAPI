using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.Interfaces.Services;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name is required.");

            var result = await _roleService.AddRoleAsync(roleName);

            if (!result.Success)
            {
                if (result.Message.Contains("exists"))
                    return BadRequest(result.Message);

                return StatusCode(500, result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Role ID is required.");

            var result = await _roleService.DeleteRoleAsync(id);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
