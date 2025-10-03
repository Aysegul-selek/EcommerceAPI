using Application.Dtos.ResponseDto;
using Application.Dtos.UserDto;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        
        Task<ApiResponseDto<object>> AddUser(CreateUserDto user);
        Task<ApiResponseDto<UserReadDto>> GetUserById(int userId);
        Task<ApiResponseDto<IEnumerable<UserReadDto>>> GetAllUsers();
        Task<ApiResponseDto<object>> UpdateUser(UpdateUserDto user, int id);
        Task<ApiResponseDto<object>> DeleteUser(long id);
        //Task<ApiResponseDto<object>> AssignRoleToUser(long userId, long roleId);
    }
}
