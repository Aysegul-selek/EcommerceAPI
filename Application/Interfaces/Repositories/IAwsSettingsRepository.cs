using Application.Dtos.aws;
using Domain.Aws_Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IAwsSettingsRepository : IRepositoryBase<AwsSetting>
    {
        Task<AwsSetting?> GetAsync();
        Task AddAwsSettingAsync(AwsSetting setting);
    }
}
