using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BackupsController(ILogger<DatabasesController> _logger, ISqlService _sqlService) : ControllerBase
    {
        [HttpGet("Backup")]
        public async Task Backup(string database)
        {
            await _sqlService.BackupAsync(database);
        }

        [HttpGet("Delete")]
        public async Task Delete(string backup)
        {
            await _sqlService.DeleteBackupByNameAsync(backup);
        }

        [HttpGet("Get")]
        public async Task<BackupDto> Get(string backup)
        {
            var result = await _sqlService.Select<BackupDto>(backup);

            return result.FirstOrDefault();
        }

        [HttpGet("GetForDatabase")]
        public List<BackupDto> GetForDatabase(string database)
        {
            var result = _sqlService.GetBackups(database);

            return result;
        }

        [HttpGet("Restore")]
        public async Task Restore(string backup, string database)
        {
            await _sqlService.RestoreDatabaseAsync(database, backup);
        }
    }
}