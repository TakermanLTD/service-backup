using Microsoft.SqlServer.Management.Smo;
using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IDatabasesService
    {
        bool Delete(string database);

        DatabaseDto Get(string database);

        List<DatabaseDto> GetAll();

        List<Database> GetAllAsEntity();

        Database GetAsEntity(string database);

        bool Create(string database);
    }
}