using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class ScheduledBackgroundService(IPackagesService _packagesService, ILogger<ScheduledBackgroundService> _logger,
        IHostEnvironment _hostEnvironment) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (_hostEnvironment.IsDevelopment() == false)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await _packagesService.CreateBackupPackages("daily");

                        _packagesService.MaintainBackups();

                        var delay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 0, 0, 0) - DateTime.Now;

                        await Task.Delay(delay, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "*Backups Service*: " + ex.GetMessage());
            }
        }
    }
}