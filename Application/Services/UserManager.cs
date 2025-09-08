using Application.Dtos.ResponseDto;
using Application.Dtos.UserDto;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Application.Services
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManager(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? CurrentUserEmail => _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c =>
                c.Type == ClaimTypes.Email ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
            )?.Value;

        private void EnsureUserAuthorized()
        {
            if (string.IsNullOrEmpty(CurrentUserEmail))
                throw new UnauthorizedException("Kullanıcı yetkili değil veya token geçersiz.");
        }

        public async Task<ApiResponseDto<object>> AddUser(CreateUserDto user)
        {
            EnsureUserAuthorized();

            var existing = await _userRepository.GetByEmailAsync(user.Email);
            if (existing != null)
                throw new ConflictException("Bu email ile zaten kayıtlı kullanıcı var.");

            var createUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            };

            await _userRepository.AddAsync(createUser);

            return new ApiResponseDto<object>
            {
                Success = true,
                Message = "Kullanıcı başarıyla eklendi",
                Data = null
            };
        }

        public async Task<ApiResponseDto<object>> UpdateUser(UpdateUserDto user, int id)
        {
            EnsureUserAuthorized();

            var userDb = await _userRepository.FindByIdAsync(id);
            if (userDb is null)
                throw new NotFoundException("Kullanıcı bulunamadı");

            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;

            await _userRepository.Update(userDb);

            return new ApiResponseDto<object>
            {
                Success = true,
                Message = "Kullanıcı güncellendi",
                Data = null
            };
        }

        public async Task<ApiResponseDto<User>> GetUserById(int userId)
        {
            EnsureUserAuthorized();

            var user = await _userRepository.FindByIdAsync(userId);
            if (user is null)
                throw new NotFoundException("Kullanıcı bulunamadı");

            return new ApiResponseDto<User>
            {
                Success = true,
                Message = "Kullanıcı getirildi",
                Data = user
            };
        }

        public async Task<ApiResponseDto<IEnumerable<User>>> GetAllUsers()
        {
            EnsureUserAuthorized();

            var users = await _userRepository.GetAllAsync();
            return new ApiResponseDto<IEnumerable<User>>
            {
                Success = true,
                Message = "Kullanıcı listesi getirildi",
                Data = users
            };
        }

        public async Task<ApiResponseDto<object>> DeleteUser(long id)
        {
            EnsureUserAuthorized();

            var userDb = await _userRepository.FindByIdAsync(id);
            if (userDb is null)
                throw new NotFoundException("Kullanıcı bulunamadı");

            await _userRepository.Delete(userDb);

            return new ApiResponseDto<object>
            {
                Success = true,
                Message = "Kullanıcı silindi",
                Data = null
            };
        }
    }
}
