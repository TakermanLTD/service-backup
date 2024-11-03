using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface ISqlService
    {
        Task BackupAllAsync();

        Task BackupAsync(string databaseName);

        Task CreateDatabaseAsync(string database);

        void DeleteBackupByNameAsync(string database, string backupFileName);

        Task DeleteFromTableAsync(string table, string condition);

        Task DeleteOldBackupsAsync(string database, int years);

        Task DropDatabaseAsync(string database);

        Task<List<DatabaseDto>> GetAllDatabasesAsync();

        List<BackupDto> GetBackups(string database);

        string GetDirectoryPath(string databaseName);

        Task MaintainBackups();

        Task OptimizeAllAsync();

        Task OptimizeDatabaseAsync(string databaseName);

        Task RestoreDatabaseAsync(string databaseName, string backupFile);

        Task<List<T>> Select<T>(string query) where T : new();
    }
}