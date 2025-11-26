using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.AppUsers;
using Sireen.Application.DTOs.Rooms;
using Sireen.Domain.Interfaces.Services;

namespace Sireen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAppUserService _userService;
        public AuthController(IAppUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateAppUserDto userDto) 
        {
            var result = await _userService.RegisterUserAsync(userDto);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateAppUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(id, userDto);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.SoftDeleteAsync(id);
            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
