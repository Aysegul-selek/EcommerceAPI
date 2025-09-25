using Application.Dtos.aws;
using Application.Dtos.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAwsSettingsService
    {
        Task<ApiResponseDto<AwsSettingsDto?>> GetAsync();
        Task<ApiResponseDto<object>> SaveOrUpdateAsync(AwsSettingsDto dto);
    }
}
