using Application.Dtos.RoleDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
       
        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
          
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("getrolesbyuserid/{userId}")]
        public async Task<IActionResult> GetRolesByUserId(long userId)
        {
            var roles = await _roleService.GetRolesByUserIdAsync(userId);
            return Ok(roles);
        }

        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<IActionResult> AssignRole(long userId, long roleId)
        {
            var result = await _roleService.AssignRoleToUser(userId, roleId);
            return Ok(result);
        }

        [HttpPost("createRoleForSystem")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRolDto roleDto)
        {
            var result = await _roleService.CreateRole(roleDto);
            return Ok(result);
        }
        [HttpDelete("removeRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] DeleteRoleDto dto)
        {
            var result = await _roleService.RemoveRoleFromUser(dto);
            return Ok(result);
        }
        [HttpDelete("deleteRoleForSystem/{roleId}")]
        public async Task<IActionResult> DeleteRole(long roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return Ok(result);
        }
    }
}
