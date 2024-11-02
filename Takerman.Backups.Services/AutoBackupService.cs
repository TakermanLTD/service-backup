using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class AutoBackupService(ISqlService _sqlService, ILogger<AutoBackupService> _logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var databases = await _sqlService.GetAllDatabasesAsync();
                    foreach (var database in databases.Where(x => x.Name.StartsWith("takerman")))
                    {
                        await _sqlService.BackupAsync(database.Name);
                    }

                    _logger.LogInformation("Daily backups finished");

                    _sqlService.MaintainBackups();

                    _logger.LogInformation("Maintenance finished");

                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.GetMessage());
            }
        }
    }
}