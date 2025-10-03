using Application.Dtos.aws;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/v1/aws")]
    [ApiController]
    public class AwsSettingsController : ControllerBase
    {
        private readonly IAwsSettingsService _awsSettingsService;

        public AwsSettingsController(IAwsSettingsService awsSettingsService)
        {
            _awsSettingsService = awsSettingsService;
        }


        [HttpGet("GetSettings")]
        public async Task<IActionResult> GetSettings()
        {
            var response = await _awsSettingsService.GetAsync();
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddSettings")]
        public async Task<IActionResult> SaveOrUpdateSettings([FromBody] AwsSettingsDto dto)
        {
            var response = await _awsSettingsService.SaveOrUpdateAsync(dto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

    }
}
