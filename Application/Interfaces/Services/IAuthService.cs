using Application.Dtos.AuthDto;
using Application.Dtos.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponseDto<LoginResponse?>> LoginAsync(LoginRequestDto request);
        Task<ApiResponseDto<object>> Register(RegisterRequestDto request);
    }
}
