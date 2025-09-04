
using Application.Dtos.UserDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task AddUser(CreateUserDto user);
        Task<User> GetUserById(int userId);
        Task<IEnumerable<User>> GetAllUsers();
        Task UpdateUser(UpdateUserDto user,int id);
        Task DeleteUser(long id);
        

    }
}
