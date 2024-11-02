using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class SqlService(IOptions<ConnectionStrings> _connectionString, IOptions<CommonConfig> _commonConfig) : ISqlService
    {
        public async Task BackupAsync(string databaseName)
        {
            var query = @$"
DECLARE @BackupFileName NVARCHAR(255); 
SET @BackupFileName = @FileName; 
BACKUP DATABASE {databaseName} TO DISK = @BackupFileName;";

            var fileName = $"{_commonConfig.Value.BackupsLocation}{databaseName}_{DateTime.Now:yy_MM_dd_HH_mm}.bak";

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

        public async Task DeleteBackupByNameAsync(string backupFileName)
        {
            var backupFilePath = Path.Combine(_commonConfig.Value.BackupsLocation, backupFileName);

            await Task.Run(() =>
            {
                if (File.Exists(backupFilePath))
                {
                    File.Delete(backupFilePath);
                }
            });
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
                if (Directory.Exists(_commonConfig.Value.BackupsLocation))
                {
                    var files = Directory.GetFiles(_commonConfig.Value.BackupsLocation, database + "*.bak", SearchOption.TopDirectoryOnly);

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

            if (Directory.Exists(_commonConfig.Value.BackupsLocation))
            {
                var files = Directory.GetFiles(_commonConfig.Value.BackupsLocation, database + "*.bak", SearchOption.TopDirectoryOnly);
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

            return backupFiles;
        }

        public async Task RestoreDatabaseAsync(string databaseName, string backupFile)
        {
            var query = $@"
                RESTORE DATABASE {databaseName}
                FROM DISK = '{_commonConfig.Value.BackupsLocation}{backupFile}'
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
    }
}