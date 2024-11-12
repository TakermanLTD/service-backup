using Microsoft.Extensions.Options;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class DashboardService(IOptions<CommonConfig> _commonConfig) : IDashboardService
    {
        public async Task<DashboardDto> GetDashboard()
        {
            var result = new DashboardDto
            {
                ScheduledFor = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 3, 0, 0),
                TotalSize = (new DirectoryInfo(_commonConfig.Value.BackupsLocation).DirectorySize() / 1024) / 1024D
            };

            return result;
        }
    }
}