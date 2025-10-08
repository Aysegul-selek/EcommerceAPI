
using Application.Dtos.UserDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
   [Authorize]
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto user)
        {
            _logger.LogInformation("AddUser endpoint çağrıldı.");
            var response = await _userService.AddUser(user);
            if(!response.Success)
            {
                _logger.LogWarning("Kullanıcı eklenemedi: {Message}", response.Message);
                return BadRequest(response);
            }
            _logger.LogInformation("Kullanıcı başarıyla eklendi.");
            return Ok(response);
            
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto user, [FromRoute] int id)
        {
            _logger.LogInformation("UpdateUser endpoint çağrıldı.");
            var response = await _userService.UpdateUser(user, id);
            if (!response.Success)
            {
                _logger.LogWarning("Kullanıcı güncellenemedi: {Message}", response.Message);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("GetAllUsers endpoint çağrıldı.");
            var response = await _userService.GetAllUsers();
            if (!response.Success)
            {
                _logger.LogWarning("Kullanıcılar alınamadı: {Message}", response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("Toplam {Count} kullanıcı bulundu.", response.Data?.Count() ?? 0);
            return Ok(response);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            _logger.LogInformation("GetUserById endpoint çağrıldı.");
            var response = await _userService.GetUserById(id);
            if (!response.Success)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: {Message}", response.Message);
                return BadRequest(response);
            }
            _logger.LogInformation("Kullanıcı başarıyla bulundu: {UserId}", id);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            _logger.LogInformation("DeleteUser endpoint çağrıldı.");
            var response = await _userService.DeleteUser(id);
            if (!response.Success)
            {
                _logger.LogWarning("Kullanıcı silinemedi: {Message}", response.Message);
                return BadRequest(response);
            }
            _logger.LogInformation("Kullanıcı başarıyla silindi: {UserId}", id);
            return Ok(response);
        }

       



    }
}
