using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class SqlService(IOptions<ConnectionStrings> _connectionString, IOptions<CommonConfig> _commonConfig, ILogger<SqlService> _logger) : ISqlService
    {
        public async Task BackupAsync(string databaseName)
        {
            var query = @$"
DECLARE @BackupFileName NVARCHAR(255);
SET @BackupFileName = @FileName;
BACKUP DATABASE {databaseName} TO DISK = @BackupFileName;";

            var directory = GetDirectoryPath(databaseName);

            var fileName = Path.Combine(directory, $"{databaseName}_{DateTime.Now:yy_MM_dd_HH_mm}.bak");

            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FileName", fileName);

            await command.ExecuteNonQueryAsync();
        }

        public async Task CreateDatabaseAsync(string database)
        {
            var query = $"CREATE DATABASE {database}";

            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);

            await command.ExecuteNonQueryAsync();
        }

        public void DeleteBackupByNameAsync(string database, string backupFileName)
        {
            var backupFilePath = Path.Combine(GetDirectoryPath(database), backupFileName);

            if (File.Exists(backupFilePath))
                File.Delete(backupFilePath);
        }

        public async Task DeleteFromTableAsync(string table, string condition)
        {
            var query = $"DELETE FROM {table} WHERE {condition}";

            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteOldBackupsAsync(string database, int years)
        {
            var threeYearsAgo = DateTime.Now.AddYears(-years);

            await Task.Run(() =>
            {
                if (Directory.Exists(GetDirectoryPath(database)))
                {
                    var files = Directory.GetFiles(GetDirectoryPath(database), database + "*.bak", SearchOption.TopDirectoryOnly);

                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        if (fileInfo.CreationTime < threeYearsAgo)
                        {
                            fileInfo.Delete();
                        }
                    }
                }
            });
        }

        public async Task DropDatabaseAsync(string database)
        {
            var query = $"DROP DATABASE {database}";

            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<DatabaseDto>> GetAllDatabasesAsync()
        {
            var databases = await Select<DatabaseDto>(@"
                SELECT
                name AS Name,
                state_desc AS State,
                recovery_model_desc AS RecoveryModel,
                (SELECT SUM(size * 8.0 / 1024)
                    FROM sys.master_files
                    WHERE type = 0 AND database_id = d.database_id) AS DataSizeMB,
                    (SELECT SUM(size * 8.0 / 1024)
                     FROM sys.master_files
                     WHERE type = 1 AND database_id = d.database_id) AS LogSizeMB
                 FROM sys.databases d");

            return databases;
        }

        public List<BackupDto> GetBackups(string database)
        {
            var backupFiles = new List<BackupDto>();

            if (Directory.Exists(GetDirectoryPath(database)))
            {
                var files = Directory.GetFiles(GetDirectoryPath(database), database + "*.bak", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    var backup = new BackupDto()
                    {
                        Created = fileInfo.CreationTime,
                        Location = fileInfo.FullName,
                        Size = fileInfo.Length / 1024,
                        Name = fileInfo.Name
                    };
                    backupFiles.Add(backup);
                }
            }

            return [.. backupFiles.OrderByDescending(x => x.Created)];
        }

        public async Task MaintainBackups()
        {
            var databases = await GetAllDatabasesAsync();
            foreach (var database in databases)
            {
                var backupFiles = new DirectoryInfo(GetDirectoryPath(database.Name))
                    .GetFiles("*.bak")
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                var dailyBackups = backupFiles.Take(10).ToList();
                var monthlyBackups = backupFiles.Where(f => f.CreationTime > DateTime.Now.AddMonths(-5)).ToList();
                var yearlyBackups = backupFiles.Where(f => f.CreationTime > DateTime.Now.AddYears(-3)).ToList();
                var backupsToKeep = dailyBackups.Concat(monthlyBackups).Concat(yearlyBackups).Distinct().ToList();

                foreach (var file in backupFiles.Except(backupsToKeep))
                {
                    file.Delete();
                }
            }
        }

        public async Task OptimizeDatabaseAsync(string databaseName)
        {
            var query = $@"
                USE {databaseName};

                -- Rebuild all indexes
                EXEC sp_MSforeachtable @command1='PRINT ''?'' DBCC DBREINDEX (''?'')';

                -- Update statistics
                EXEC sp_MSforeachtable @command1='UPDATE STATISTICS ?';

                -- Shrink the database to reclaim unused space (optional)
                DBCC SHRINKDATABASE ({databaseName});";

            try
            {
                await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
                await connection.OpenAsync();

                await using var command = new SqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
            }
        }

        public async Task RestoreDatabaseAsync(string databaseName, string backupFile)
        {
            var file = Path.Combine(GetDirectoryPath(databaseName), backupFile);
            var query = $@"
                RESTORE DATABASE {databaseName}
                FROM DISK = '{file}'
                WITH REPLACE";

            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> Select<T>(string query) where T : new()
        {
            await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
            await connection.OpenAsync();
            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            var resultList = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            while (reader.Read())
            {
                var obj = new T();
                foreach (var property in properties)
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        var value = reader[property.Name];

                        property.SetValue(obj, value, null);
                    }
                }
                resultList.Add(obj);
            }

            return resultList;
        }

        public string GetDirectoryPath(string databaseName)
        {
            var directory = Path.Combine(_commonConfig.Value.BackupsLocation, databaseName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }
    }
}