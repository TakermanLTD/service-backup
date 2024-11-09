using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class ScheduledBackgroundService(IPackagesService _sqlService, ILogger<ScheduledBackgroundService> _logger,
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
                        // zips the folder with the mssql backups and moves the zip to the folder with date in the backups folder in the volumes
                        // zips the folder with the mysql backups and moves the zip to the folder with the date in the backups folder in the volumes
                        // copies the files that i need from the volumes to a volumes folder in the date folder and zips the folder. then removes the folder
                        // after that syncs the backups folder to google drive

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