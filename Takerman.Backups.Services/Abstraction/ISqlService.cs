using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface ISqlService
    {
        Task BackupAsync(string databaseName);

        Task CreateDatabaseAsync(string database);

        Task DeleteBackupByNameAsync(string backupFileName);

        Task DeleteFromTableAsync(string table, string condition);

        Task DeleteOldBackupsAsync(string database, int years);

        Task DropDatabaseAsync(string database);

        Task<List<DatabaseDto>> GetAllDatabasesAsync();

        List<BackupDto> GetBackups(string database);

        void MaintainBackups();

        Task OptimizeDatabaseAsync(string databaseName);

        Task RestoreDatabaseAsync(string databaseName, string backupFile);

        Task<List<T>> Select<T>(string query) where T : new();
    }
}