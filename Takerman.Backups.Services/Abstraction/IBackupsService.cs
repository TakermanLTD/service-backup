using Microsoft.SqlServer.Management.Smo;
using System.Data;
using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IBackupsService
    {
        List<BackupDto> BackupAll(bool incremental);

        BackupDto Backup(string database, bool incremental);

        bool Delete(string backupName);

        BackupDto Get(string backup);

        List<BackupDto> GetAll(string database = "");

        bool Restore(string backup, string database);

        bool DeleteAll(string database);
    }
}