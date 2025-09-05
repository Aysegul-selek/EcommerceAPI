using Application.Dtos.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequestDto request);
        Task Register(RegisterRequestDto request);
    }
}
