
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto user)
        {
            var response = await _userService.AddUser(user);
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto user, [FromRoute] int id)
        {
            var response = await _userService.UpdateUser(user, id);
            return Ok(response);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            return Ok(response);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var response = await _userService.GetUserById(id);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var response = await _userService.DeleteUser(id);
            return Ok(response);
        }

       



    }
}
