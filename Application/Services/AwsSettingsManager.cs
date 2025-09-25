using Application.Dtos.aws;
using Application.Dtos.ResponseDto;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AwsSettingsManager : IAwsSettingsService
    {
        private readonly IAwsSettingsRepository _awsSettingsRepository;

        public AwsSettingsManager(IAwsSettingsRepository awsSettingsRepository)
        {
            _awsSettingsRepository = awsSettingsRepository;
        }

        public async Task<ApiResponseDto<AwsSettingsDto?>> GetAsync()
        {
           var entity = await _awsSettingsRepository.GetAsync();
            if (entity == null)
                throw new NotFoundException("AWS ayarları bulunamadı.");
            var dto = new AwsSettingsDto
            {
                
                AccessKey = entity.AccessKey,
                SecretKey = entity.SecretKey,
                Region = entity.Region,
                BucketName = entity.BucketName
            };
            return new ApiResponseDto<AwsSettingsDto?>
            {
                Success = true,
                Message = "AWS ayarları başarıyla getirildi.",
                Data = dto,
                ErrorCodes = null
            };
        }

        public async Task<ApiResponseDto<object>> SaveOrUpdateAsync(AwsSettingsDto dto)
        {

            var existingSetting = await _awsSettingsRepository.GetAsync();
            if (existingSetting != null)
            {
                // Güncelleme işlemi
                existingSetting.AccessKey = dto.AccessKey;
                existingSetting.SecretKey = dto.SecretKey;
                existingSetting.Region = dto.Region;
                existingSetting.BucketName = dto.BucketName;
                await _awsSettingsRepository.Update(existingSetting);
                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "AWS ayarları başarıyla güncellendi.",
                    Data = null,
                    ErrorCodes = null
                };
            }
            else
            {
                // Yeni kayıt ekleme işlemi
                var newSetting = new Domain.Aws_Settings.AwsSetting
                {
                    AccessKey = dto.AccessKey,
                    SecretKey = dto.SecretKey,
                    Region = dto.Region,
                    BucketName = dto.BucketName,
                    IsDeleted = false
                };
                await _awsSettingsRepository.AddAsync(newSetting);
                return new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "AWS ayarları başarıyla kaydedildi.",
                    Data = null,
                    ErrorCodes = null
                };
            }
        }
    }
}
