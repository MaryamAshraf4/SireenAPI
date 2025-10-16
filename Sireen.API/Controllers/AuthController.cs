using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sireen.Application.DTOs.AppUsers;
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
    }
}
