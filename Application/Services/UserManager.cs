using Application.Dtos.UserDto;
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
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUser(CreateUserDto user)
        {
            var createUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false

            };
           await _userRepository.AddAsync(createUser);
        }

        public async Task DeleteUser(long Id)
        {
            var userDb = await _userRepository.FindByIdAsync(Id);
            if(userDb is null)
                throw new Exception("Kullanıcı bulunamadı");
            await _userRepository.Delete(userDb);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllAsync();

        }

        public Task<User> GetUserById(int userId)
        {
            var user = _userRepository.FindByIdAsync(userId);
            if (user is null)
                throw new Exception("Kullanıcı bulunamadı");
            return user;
        }

       

        public async Task UpdateUser(UpdateUserDto user,int id)
        {
            var userDb = await _userRepository.FindByIdAsync(id);
            if (userDb is null)
                throw new Exception("Kullanıcı bulunamadı");
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.UpdatedDate = DateTime.UtcNow;
            userDb.UpdatedDate = DateTime.UtcNow;
            await _userRepository.Update(userDb);
        }
    }
}
