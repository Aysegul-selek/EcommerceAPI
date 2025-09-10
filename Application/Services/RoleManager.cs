using Application.Dtos.ResponseDto;
using Application.Dtos.RoleDto;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleManager : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        public RoleManager(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponseDto<IEnumerable<RoleReadDto>>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            if (roles == null || !roles.Any())
            {
                return new ApiResponseDto<IEnumerable<RoleReadDto>>
                {
                    Success = false,
                    Data = null,
                    ErrorCodes = ErrorCodes.NotFound,
                    Message = "Hiç rol bulunamadı."
                };
            }
            var roleDtos = roles.Select(r => new RoleReadDto
            {
                Id = r.Id,
                Name = r.Name,
                UserCount = r.UserRoles?.Count ?? 0
            }).ToList();
            return new ApiResponseDto<IEnumerable<RoleReadDto>>
            {
                Success = true,
                Data = roleDtos,
                ErrorCodes = null,
                Message = "Roller başarıyla getirildi."
            };
        }

        public async Task<ApiResponseDto<IEnumerable<string>>> GetRolesByUserIdAsync(long userId)
        {
            var roles = await _roleRepository.GetRolesByUserIdAsync(userId);
            if (roles == null || !roles.Any())
            {
                return new ApiResponseDto<IEnumerable<string>>
                {
                    Success = false,
                    Data = null,
                    ErrorCodes = ErrorCodes.NotFound,
                    Message = "Kullanıcıya ait rol bulunamadı."
                };
            }
            return new ApiResponseDto<IEnumerable<string>>
            {
                Success = true,
                Data = roles,
                ErrorCodes = null,
                Message = "Kullanıcıya ait roller başarıyla getirildi."
            };

        }

        public async Task<ApiResponseDto<object>> AssignRoleToUser(long userId, long roleId)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null) throw new NotFoundException("Kullanıcı bulunamadı");

            var role = await _roleRepository.FindByIdAsync(roleId);
            if (role == null) throw new NotFoundException("Rol bulunamadı");

            // Zaten bu role sahip mi?
            var existing = await _roleRepository.GetRolesByUserIdAsync(userId);
            if (existing.Contains(role.Name))
            {
                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = $"Kullanıcı zaten {role.Name} rolüne sahip.",
                    Data = null
                };
            }

            // sadece UserRoles tablosuna ekle
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userRepository.AddUserRoleAsync(userId,roleId);

            return new ApiResponseDto<object>
            {
                Success = true,
                Message = $"{role.Name} rolü kullanıcıya atandı",
                Data = null
            };
        }
    



    }
}
