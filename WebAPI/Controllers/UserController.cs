using Application.Dtos.UserDto;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto user)
        {
            await _userService.AddUser(user);
            return Ok("kullanıcı başarılı bir şekilde eklendi");
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            await _userService.DeleteUser(id);
            return Ok("Kullanıcı başarılı bir şekilde silindi");
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto user, [FromRoute] int id)
        {
            await _userService.UpdateUser(user,id);
            return Ok("Kullanıcı başarılı bir şekilde güncellendi");
        }
    }
}
