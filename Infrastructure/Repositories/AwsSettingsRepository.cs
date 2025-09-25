using Application.Dtos.aws;
using Application.Interfaces.Repositories;
using Domain.Aws_Settings;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AwsSettingsRepository : RepositoryBase<AwsSetting>, IAwsSettingsRepository
    {
        public AwsSettingsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<AwsSetting?> GetAsync()
        {
            
            return await _context.AwsSettings.AsNoTracking().FirstOrDefaultAsync(a => !a.IsDeleted);
        }

        public async Task AddAwsSettingAsync(AwsSetting setting)
        {
            await _context.AwsSettings.AddAsync(setting);
        }
    }
}
