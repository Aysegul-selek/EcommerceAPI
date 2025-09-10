using Application.Dtos.ResponseDto;
using Application.Dtos.RoleDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ApiResponseDto<IEnumerable<RoleReadDto>>> GetAllAsync();
        Task<ApiResponseDto<IEnumerable<string>>> GetRolesByUserIdAsync(long userId);
        Task<ApiResponseDto<object>> AssignRoleToUser(long userId, long roleId);
    }
}
