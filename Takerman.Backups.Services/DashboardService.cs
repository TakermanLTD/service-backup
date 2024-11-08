using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class DashboardService : IDashboardService
    {
        public async Task<DashboardDto> GetDashboard()
        {
            return new DashboardDto();
        }
    }
}