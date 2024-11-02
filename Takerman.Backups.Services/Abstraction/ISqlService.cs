using Microsoft.Data.SqlClient;
using System.Reflection;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface ISqlService
    {
        Task BackupAsync(string databaseName);

        Task CreateDatabaseAsync(string database);

        void DeleteBackupByNameAsync(string database, string backupFileName);

        Task DeleteFromTableAsync(string table, string condition);

        Task DeleteOldBackupsAsync(string database, int years);

        Task DropDatabaseAsync(string database);

        Task<List<DatabaseDto>> GetAllDatabasesAsync();

        List<BackupDto> GetBackups(string database);

        Task MaintainBackups();

        Task OptimizeDatabaseAsync(string databaseName);

        Task RestoreDatabaseAsync(string databaseName, string backupFile);

        Task<List<T>> Select<T>(string query) where T : new();

        string GetDirectoryPath(string databaseName);
    }
}