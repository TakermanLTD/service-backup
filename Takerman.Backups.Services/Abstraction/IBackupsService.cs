using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IBackupsService
    {
        BackupDto Backup(string database);

        List<BackupDto> BackupAll();

        bool Delete(string backupName);

        bool DeleteAll(string database);

        BackupDto Get(string backup);

        List<BackupDto> GetAll(string database = "");

        bool Restore(string backup, string database);
    }
}