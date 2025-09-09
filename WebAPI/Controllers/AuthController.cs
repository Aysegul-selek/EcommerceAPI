using Application.Dtos.AuthDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await _authService.Register(request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
