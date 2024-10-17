using Takerman.Backup.Models.DTOs;

namespace Takerman.Backup.Services
{
    public interface IDatabaseService
    {
        Task Backup(string name);

        Task<DatabaseDto> Get(string name);

        Task<List<DatabaseDto>> GetAll();
    }
}