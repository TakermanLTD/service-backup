using Microsoft.Extensions.Logging;
using Takerman.Backup.Models.DTOs;

namespace Takerman.Backup.Services
{
    public class DatabaseService(ILogger<DatabaseService> _logger) : IDatabaseService
    {
        public Task Backup(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DatabaseDto> Get(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<DatabaseDto>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}