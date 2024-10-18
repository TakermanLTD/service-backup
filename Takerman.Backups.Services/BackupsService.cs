using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;
using System.Data;
using Takerman.Backups.Models.Configuration;
using Microsoft.Extensions.Options;


namespace Takerman.Backupss.Services
{
    public class BackupsService : IBackupsService
    {
        public BackupsService(ILogger<BackupsService> _logger, IOptions<ConnectionStrings> _connectionStrings)
        {
            DbServer = new Server();
            DbServer.ConnectionContext.ConnectionString = _connectionStrings.Value.DefaultConnection;
            DbServer.ConnectionContext.Connect();
        }

        public Server DbServer { get; }

        public List<BackupDto> BackupAll(bool incremental)
        {
            var databases = new List<Database>();

            foreach (Database database in DbServer.Databases)
                databases.Add(database);

            var result = new List<BackupDto>();

            foreach (var database in databases)
                result.Add(Backup(database.Name, incremental));

            return result;
        }

        public BackupDto Backup(string database, bool incremental)
        {
            var backupName = $"{database}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}.bak";
            var backup = new Backup
            {
                Database = database,
                Incremental = incremental,
                Action = BackupActionType.Database,
                LogTruncation = BackupTruncateLogType.Truncate,
                Initialize = true
            };

            backup.Devices.AddDevice(backupName, DeviceType.File);

            backup.SqlBackup(DbServer);

            var result = new BackupDto()
            {
                Name = backupName
            };

            return result;
        }

        public bool Delete(string backupName)
        {
            var backup = GetAll().FirstOrDefault(x => x.Name == backupName);
            File.Delete(backup.Location);

            return true;
        }

        public BackupDto Get(string backup)
        {
            return GetAll().FirstOrDefault(x => x.Name.ToLower() == backup.ToLower());
        }

        public List<BackupDto> GetAll(string database = "")
        {
            var result = new List<BackupDto>();

            string sql = $"SELECT mf.media_set_id AS ID, mf.physical_device_name AS Location, bs.backup_size AS Size FROM msdb.dbo.backupmediafamily mf INNER JOIN msdb.dbo.backupset bs ON mf.media_set_id = bs.media_set_id";

            if (!string.IsNullOrEmpty(database))
                sql += $" WHERE bs.database_name = '{database}'";

            DataSet ds = DbServer.ConnectionContext.ExecuteWithResults(sql);
            DataTable backupFiles = ds.Tables[0];

            foreach (DataRow row in backupFiles.Rows)
            {
                var location = (string)row["Location"];

                result.Add(new BackupDto()
                {
                    Id = (int)row["ID"],
                    Location = location,
                    Size = ((decimal)row["Size"])/ 1024,
                    Name = location[location.LastIndexOf('/')..]
                });
            }

            return result;
        }

        public bool Restore(string backup, string database)
        {
            var restore = new Restore
            {
                Action = RestoreActionType.Database,
                Database = database,
                ReplaceDatabase = true,
            };
            restore.Devices.AddDevice(backup, DeviceType.File);

            restore.SqlRestore(DbServer);

            return true;
        }

        public bool DeleteAll(string database)
        {
            Database db = DbServer.Databases[database];

            DataTable backupSets = db.EnumBackupSets();

            return true;
        }
    }
}