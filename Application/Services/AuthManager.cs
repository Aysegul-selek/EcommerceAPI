using Application.Dtos.AuthDto;
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

        public async Task<LoginResponse?> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || user.Password != request.Password)
            {
                return null; // Kullanıcı bulunamadı veya şifre yanlış hata da dönebilir
            }
            var token = _jwtService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                GecerlilikTarihi = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task Register(RegisterRequestDto request)
        {
            var userDb = await _userRepository.GetByEmailAsync(request.Email);
            if (userDb != null)
            {
                throw new Exception("Bu Email kullanılmaktadır"); // Hata fırlatabilir veya özel bir sonuç dönebilir
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

        }
    }
}
