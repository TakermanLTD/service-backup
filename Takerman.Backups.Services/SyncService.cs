using Microsoft.Extensions.Logging;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class SyncService(ILogger<SyncService> _logger) : ISyncService
    {
        public string Sync()
        {
            try
            {
                var result = "sshpass -p 'Hakerman91!' ssh -t -t -p 1991 root@takerman.net && rclone sync /home/takerman/volumes/mssql/data/ google-drive:projects/backups && dupdate".ExecuteCommand();
                _logger.LogInformation("Shell execution: {ShellExecution}", result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.GetMessage());

                return ex.GetMessage();
            }
        }
    }
}