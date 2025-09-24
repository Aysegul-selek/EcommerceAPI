using Application.Dtos.ResponseDto;
using Application.Dtos.UserDto;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<UserManager> _logger;

        public UserManager(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<UserManager> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<ApiResponseDto<object>> AddUser(CreateUserDto user)
        {
            try
            {
                _logger.LogInformation("AddUser endpoint çağrıldı. Email: {Email}", user.Email);

                var existing = await _userRepository.GetByEmailAsync(user.Email);
                if (existing != null)
                {
                    _logger.LogWarning("ConflictException oluştu. Email zaten kayıtlı: {Email}", user.Email);
                    throw new ConflictException("Bu email ile zaten kayıtlı kullanıcı var.");
                }

                var createUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };

                await _userRepository.AddAsync(createUser);
                _logger.LogInformation("Yeni kullanıcı eklendi. Email: {Email}", user.Email);

                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Kullanıcı başarıyla eklendi",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı ekleme sırasında hata oluştu. Email: {Email}", user.Email);
                throw; // GlobalExceptionMiddleware tarafından yakalanacak
            }
        }

        public async Task<ApiResponseDto<object>> UpdateUser(UpdateUserDto user, int id)
        {
            try
            {
                _logger.LogInformation("UpdateUser endpoint çağrıldı. UserId: {UserId}", id);

                var userDb = await _userRepository.FindByIdAsync(id);
                if (userDb is null)
                {
                    _logger.LogWarning("NotFoundException: Kullanıcı bulunamadı. UserId: {UserId}", id);
                    throw new NotFoundException("Kullanıcı bulunamadı");
                }

                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;

                await _userRepository.Update(userDb);
                _logger.LogInformation("Kullanıcı güncellendi. UserId: {UserId}", id);

                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Kullanıcı güncellendi",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme sırasında hata oluştu. UserId: {UserId}", id);
                throw;
            }
        }

        public async Task<ApiResponseDto<User>> GetUserById(int userId)
        {
            try
            {
                _logger.LogInformation("GetUserById çağrıldı. UserId: {UserId}", userId);

                var user = await _userRepository.FindByIdAsync(userId);
                if (user is null)
                {
                    _logger.LogWarning("NotFoundException: Kullanıcı bulunamadı. UserId: {UserId}", userId);
                    throw new NotFoundException("Kullanıcı bulunamadı");
                }

                _logger.LogInformation("Kullanıcı getirildi. UserId: {UserId}", userId);
                return new ApiResponseDto<User>
                {
                    Success = true,
                    Message = "Kullanıcı getirildi",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirme sırasında hata oluştu. UserId: {UserId}", userId);
                throw;
            }
        }

        public async Task<ApiResponseDto<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("GetAllUsers endpoint çağrıldı.");

                var users = await _userRepository.GetAllAsync();

                _logger.LogInformation("Kullanıcı listesi getirildi. Toplam kullanıcı: {Count}", users.Count());
                return new ApiResponseDto<IEnumerable<User>>
                {
                    Success = true,
                    Message = "Kullanıcı listesi getirildi",
                    Data = users
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı listesi getirilirken hata oluştu.");
                throw;
            }
        }

        public async Task<ApiResponseDto<object>> DeleteUser(long id)
        {
            try
            {
                _logger.LogInformation("DeleteUser endpoint çağrıldı. UserId: {UserId}", id);

                var userDb = await _userRepository.FindByIdAsync(id);
                if (userDb is null)
                {
                    _logger.LogWarning("NotFoundException: Kullanıcı bulunamadı. UserId: {UserId}", id);
                    throw new NotFoundException("Kullanıcı bulunamadı");
                }

                await _userRepository.Delete(userDb);
                _logger.LogInformation("Kullanıcı silindi. UserId: {UserId}", id);

                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Kullanıcı silindi",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silme sırasında hata oluştu. UserId: {UserId}", id);
                throw;
            }
        }
    }
}
