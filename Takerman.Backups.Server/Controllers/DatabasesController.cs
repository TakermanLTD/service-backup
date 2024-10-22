using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabasesController(ILogger<DatabasesController> _logger, IDatabasesService _databaseService) : ControllerBase
    {
        [HttpGet("Create")]
        public IActionResult Create(string database)
        {
            var result = _databaseService.Create(database);

            return Ok(result);
        }

        [HttpGet("Delete")]
        public IActionResult Delete(string database)
        {
            var result = _databaseService.Delete(database);

            return Ok(result);
        }

        [HttpGet("Get")]
        public IActionResult Get(string name)
        {
            var result = _databaseService.Get(name);

            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            _logger.LogCritical("asdasd qweqwe zxczxc");
            var result = _databaseService.GetAll();

            return Ok(result);
        }
    }
}