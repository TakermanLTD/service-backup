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
                        await _packagesService.CreateBackupPackages();

                        var minutesToDelay = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, 3, 0, 0) - DateTime.Now).Minutes;
                        
                        await Task.Delay(minutesToDelay, stoppingToken);
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