using Application.Dtos.AuthDto;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthManager : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(IUserRepository userRepository, IJwtService jwtService, IRoleRepository roleRepository, ILogger<AuthManager> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<ApiResponseDto<LoginResponse?>> LoginAsync(LoginRequestDto request)
        {
            try
            {
                _logger.LogInformation("Giriş denemesi başlatıldı. Email: {Email}", request.Email);

                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null || user.Password != request.Password)
                {
                    _logger.LogWarning("Geçersiz giriş denemesi. Email: {Email}", request.Email);
                    return new ApiResponseDto<LoginResponse?>
                    {
                        Data = null,
                        Success = false,
                        Message = "Geçersiz email veya şifre",
                        ErrorCodes = ErrorCodes.NotFound
                    };
                }

                _logger.LogInformation("Kullanıcı bulundu. Email: {Email}", request.Email);

                var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);
                _logger.LogInformation("Kullanıcının rolleri alındı. Email: {Email}, Rol sayısı: {RoleCount}", request.Email, roles.Count());

                var token = _jwtService.GenerateToken(user, roles);
                _logger.LogInformation("JWT token oluşturuldu. Email: {Email}", request.Email);

                var loginResponse = new LoginResponse
                {
                    Token = token,
                    GecerlilikTarihi = DateTime.UtcNow.AddHours(1)
                };

                _logger.LogInformation("Giriş başarılı. Email: {Email}", request.Email);

                return new ApiResponseDto<LoginResponse?>
                {
                    Data = loginResponse,
                    Success = true,
                    Message = "Giriş başarılı",
                    ErrorCodes = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş sırasında beklenmeyen bir hata oluştu. Email: {Email}", request.Email);
                return new ApiResponseDto<LoginResponse?>
                {
                    Data = null,
                    Success = false,
                    Message = "Giriş sırasında bir hata oluştu",
                    ErrorCodes = ErrorCodes.ServerError
                };
            }
        }

        public async Task<ApiResponseDto<object>> Register(RegisterRequestDto request)
        {
            try
            {
                _logger.LogInformation("Kayıt denemesi başlatıldı. Email: {Email}", request.Email);

                var userDb = await _userRepository.GetByEmailAsync(request.Email);
                if (userDb != null)
                {
                    _logger.LogWarning("Kayıt başarısız. Email zaten kayıtlı: {Email}", request.Email);
                    return new ApiResponseDto<object>
                    {
                        Data = null,
                        Success = false,
                        Message = "Bu email zaten kayıtlı",
                        ErrorCodes = ErrorCodes.Conflict
                    };
                }

                var newUser = new User
                {
                    Email = request.Email,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedDate = DateTime.UtcNow
                };

                await _userRepository.AddAsync(newUser);
                _logger.LogInformation("Yeni kullanıcı kaydedildi. Email: {Email}", request.Email);

                return new ApiResponseDto<object>
                {
                    Data = null,
                    Success = true,
                    Message = "Kayıt başarılı",
                    ErrorCodes = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt sırasında beklenmeyen bir hata oluştu. Email: {Email}", request.Email);
                return new ApiResponseDto<object>
                {
                    Data = null,
                    Success = false,
                    Message = "Kayıt sırasında bir hata oluştu",
                    ErrorCodes = ErrorCodes.ServerError
                };
            }
        }
    }
}
