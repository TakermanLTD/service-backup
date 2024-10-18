using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using Takerman.Backups.Models.DTOs;
using System.Linq;
using Takerman.Backups.Services.Abstraction;
using Takerman.Backups.Models.Configuration;
using Microsoft.Extensions.Options;

namespace Takerman.Backupss.Services
{
    public class DatabasesService : IDatabasesService
    {
        public DatabasesService(ILogger<DatabasesService> _logger, IOptions<ConnectionStrings> _connectionStrings)
        {
            DbServer = new Server();
            DbServer.ConnectionContext.ConnectionString = _connectionStrings.Value.DefaultConnection;
            DbServer.ConnectionContext.Connect();
        }

        public Server DbServer { get; }

        public bool Delete(string database)
        {
            var result = GetAsEntity(database);
            if (result != null)
            {
                result.DropIfExists();
                return true;
            }
            else
            {
                return false;
            }
        }

        public DatabaseDto Get(string database)
        {
            return GetAll().FirstOrDefault(x => x.Name.ToLower() == database.ToLower());
        }

        public List<DatabaseDto> GetAll()
        {
            var result = GetAllAsEntity().Select(x => new DatabaseDto()
            {
                Name = x.Name,
                Size = x.Size,
                Location = x.PrimaryFilePath
            }).ToList();

            return result;
        }

        public List<Database> GetAllAsEntity()
        {
            var result = new List<Database>();

            foreach (Database database in DbServer.Databases)
                result.Add(database);

            return result;
        }

        public Database GetAsEntity(string database)
        {
            return GetAllAsEntity().FirstOrDefault(x => x.Name.ToLower() == database.ToLower());
        }

        public bool Create(string database)
        {
            var db = new Database(DbServer, database);
            db.Create();
            return true;
        }
    }
}