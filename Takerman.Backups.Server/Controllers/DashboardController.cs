using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController(ILogger<DashboardController> _logger, IDashboardService _dashboardService) : ControllerBase
    {
        [HttpGet("Get")]
        public async Task<DashboardDto> Get()
        {
            return await _dashboardService.GetDashboard();
        }
    }
}