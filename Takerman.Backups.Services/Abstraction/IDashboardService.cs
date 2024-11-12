using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IDashboardService
    {
        DashboardDto GetDashboard();
    }
}