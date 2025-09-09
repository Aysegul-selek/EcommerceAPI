using Application.Dtos.ResponseDto;
using Application.Dtos.UserDto;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;


namespace Application.Services
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponseDto<object>> AddUser(CreateUserDto user)
        {
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
