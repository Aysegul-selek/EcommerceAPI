using Application.Dtos.AuthDto;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;


namespace Application.Services
{
    public class AuthManager : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthManager(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<ApiResponseDto<LoginResponse?>> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || user.Password != request.Password)
            {
                return new ApiResponseDto<LoginResponse?>
                {
                    Data = null,
                    Success = false,
                    Message = "Geçersiz email veya şifre",
                    ErrorCodes = ErrorCodes.NotFound
                };
            }
            var token = _jwtService.GenerateToken(user);

            var loginResponse= new LoginResponse
            {
                Token = token,
                GecerlilikTarihi = DateTime.UtcNow.AddHours(1)
            };

            return new ApiResponseDto<LoginResponse?>
            {
                Data = loginResponse,
                Success = true,
                Message = "Giriş başarılı",
                ErrorCodes = null
            };
        }

        public async Task<ApiResponseDto<object>> Register(RegisterRequestDto request)
        {
            var userDb = await _userRepository.GetByEmailAsync(request.Email);
            if (userDb != null)
            {
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
            return new ApiResponseDto<object>
            {
                Data = null,
                Success = true,
                Message = "Kayıt başarılı",
                ErrorCodes = null
            };


        }
    }
}
