using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabasesController(ILogger<DatabasesController> _logger, ISqlService _sqlService) : ControllerBase
    {
        [HttpGet("Create")]
        public async Task Create(string database)
        {
            await _sqlService.CreateDatabaseAsync(database);
        }

        [HttpGet("Delete")]
        public async Task Delete(string database)
        {
            await _sqlService.DropDatabaseAsync(database);
        }

        [HttpGet("GetAll")]
        public async Task<List<DatabaseDto>> GetAll()
        {
            var result = await _sqlService.GetAllDatabasesAsync();

            return result;
        }

        [HttpGet("Optimize")]
        public async Task Optimize(string database)
        {
            await _sqlService.OptimizeDatabaseAsync(database);
        }
    }
}