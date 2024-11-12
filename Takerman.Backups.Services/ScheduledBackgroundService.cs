using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class ScheduledBackgroundService(IPackagesService _packagesService, ISyncService _syncService, ILogger<ScheduledBackgroundService> _logger,
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

                        await _syncService.Sync();

                        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "**Backups Service**: " + ex.GetMessage());
            }
        }
    }
}