using Microsoft.AspNetCore.Mvc;
using Takerman.Backup.Services;

namespace Takerman.Backup.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController(ILogger<DatabaseController> _logger, IDatabaseService _databaseService) : ControllerBase
    {
        [HttpGet("Backup")]
        public async Task<IActionResult> Backup(string name)
        {
            await _databaseService.Backup(name);

            return Ok();
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(string name)
        {
            var result = await _databaseService.Get(name);

            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _databaseService.GetAll();

            return Ok(result);
        }
    }
}