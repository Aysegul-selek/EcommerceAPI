using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

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
    }
}
