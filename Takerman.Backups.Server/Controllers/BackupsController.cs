using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BackupsController(ILogger<DatabasesController> _logger, IBackupsService _backupsService) : ControllerBase
    {
        [HttpGet("Get")]
        public IActionResult Get(string backup)
        {
            var result = _backupsService.Get(backup);

            return Ok(result);
        }

        [HttpGet("GetForDatabase")]
        public IActionResult GetForDatabase(string database)
        {
            var result = _backupsService.GetAll(database);

            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _backupsService.GetAll();

            return Ok(result);
        }

        [HttpGet("Backup")]
        public IActionResult Backup(string database, bool incremental)
        {
            var result = _backupsService.Backup(database, incremental);

            return Ok(result);
        }

        [HttpGet("BackupAll")]
        public IActionResult BackupAll(bool incremental)
        {
            var result = _backupsService.BackupAll(incremental);

            return Ok(result);
        }

        [HttpGet("Restore")]
        public IActionResult Restore(string backup, string database)
        {
            var result = _backupsService.Restore(backup, database);

            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string backup)
        {
            var result = _backupsService.Delete(backup);

            return Ok(result);
        }

        [HttpGet("DeleteAll")]
        public IActionResult DeleteAll(string database)
        {
            var result = _backupsService.DeleteAll(database);

            return Ok(result);
        }
    }
}