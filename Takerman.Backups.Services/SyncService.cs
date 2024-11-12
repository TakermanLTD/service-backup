using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class SyncService(ILogger<SyncService> _logger) : ISyncService
    {
        public async Task Sync()
        {
            _logger.LogInformation("The sync has been called without an implementation.");

            await Task.Delay(100);
        }
    }
}