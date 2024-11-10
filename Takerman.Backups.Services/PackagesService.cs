using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Reflection;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;
using Takerman.Extensions;

namespace Takerman.Backups.Services
{
    public class PackagesService(IOptions<ConnectionStrings> _connectionString, IOptions<CommonConfig> _commonConfig, ILogger<PackagesService> _logger) : IPackagesService
    {
        public async Task BackupDatabaseAsync(string databaseName)
        {
            try
            {
                var query = @$"
                DECLARE @BackupFileName NVARCHAR(255);
                SET @BackupFileName = @FileName;
                BACKUP DATABASE {databaseName} TO DISK = @BackupFileName;";

                var fileName = Path.Combine(_commonConfig.Value.MicrosoftSqlLocation, $"{databaseName}.bak");

                await using var connection = new SqlConnection(_connectionString.Value.DefaultConnection);
                await connection.OpenAsync();

                await using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FileName", fileName);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.GetMessage());
            }
        }

        public async Task CreateBackupPackage(string project)
        {
            var packages = GetProjectPackageEntries();

            var package = packages.FirstOrDefault(x => x.Name.ToLower() == project.ToLower());

            var packageDirectory = Path.Combine(_commonConfig.Value.BackupsLocation, package.Name, $"{package.Name}_{DateTime.Now:yyyy_MM_dd_hh_mm}");
            if (!Directory.Exists(packageDirectory))
                Directory.CreateDirectory(packageDirectory);

            foreach (var entry in package.BackupEntries)
            {
                switch (entry.Type)
                {
                    case BackupEntryType.MicrosoftSQL:
                        if (await DatabaseExists(entry.Source))
                        {
                            await BackupDatabaseAsync(entry.Source);
                            File.Move(Path.Combine(_commonConfig.Value.MicrosoftSqlLocation, $"{entry.Source}.bak"), packageDirectory);
                        }
                        else
                        {
                            _logger.LogWarning("**Backups**: Tried to backup a microsoft database for a package, but the database does not exits");
                        }
                        break;

                    case BackupEntryType.MySQL:
                    case BackupEntryType.MariaDB:
                        if (await DatabaseExists(entry.Source))
                        {
                            await BackupDatabaseAsync(entry.Source);
                            File.Move(Path.Combine(_commonConfig.Value.MariaDBLocation, $"{entry.Source}.sql"), packageDirectory);
                        }
                        else
                        {
                            _logger.LogWarning("**Backups**: Tried to backup a mariadb database for a package, but the database does not exits");
                        }
                        break;

                    case BackupEntryType.SQLite:
                        break;

                    case BackupEntryType.Folder:
                    case BackupEntryType.File:
                        var filePath = Path.Combine(_commonConfig.Value.VolumesLocation, entry.Source);
                        if (File.Exists(filePath))
                        {
                            File.Copy(Path.Combine(_commonConfig.Value.VolumesLocation, entry.Source), packageDirectory);
                        }
                        else
                        {
                            _logger.LogWarning("**Backups**: Tried to copy files for a package, but the files does not exits");
                        }
                        break;
                }
            }

            ZipFile.CreateFromDirectory(packageDirectory, packageDirectory + ".zip", CompressionLevel.Optimal, true);

            Directory.Delete(packageDirectory, true);
        }

        public async Task CreateBackupPackages()
        {
            var packages = GetProjectPackageEntries();

            foreach (var package in packages)
            {
                await CreateBackupPackage(package.Name);
            }
        }

        public async Task<bool> DatabaseExists(string database)
        {
            var result = await Select<dynamic>($@"SELECT * FROM master.sys.databases WHERE name = N'{database}'");

            return result?.Count > 0;
        }

        public void DeletePackage(string project, string package)
        {
            var path = Path.Combine(_commonConfig.Value.BackupsLocation, project, package);

            File.Delete(path);
        }

        public List<ProjectDto> GetAll()
        {
            var result = new List<ProjectDto>();

            if (Directory.Exists(_commonConfig.Value.BackupsLocation))
            {
                var directories = Directory.GetDirectories(_commonConfig.Value.BackupsLocation);

                foreach (var dir in directories)
                {
                    var files = Directory.GetFiles(dir);
                    var entry = new ProjectDto()
                    {
                        Name = dir,
                        PackagesCount = files.Length,
                        TotalSizeMB = files.Sum(x => new FileInfo(x).Length / 1024)
                    };

                    result.Add(entry);
                }
            }

            return result;
        }

        public List<PackageEntriesDto> GetProjectPackageEntries()
        {
            return
            [
                new(){
                    Name = "Printing",
                    BackupEntries =
                    [
                        new(){ Type = BackupEntryType.MariaDB, Source = "takerprint" },
                        new(){ Type = BackupEntryType.Folder, Source = "takerprint/wp-content" }
                    ]
                },
                new(){
                    Name = "Dating",
                    BackupEntries =
                    [
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_dating_bg" },
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_dating_ro" },
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_dating_ru" },
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_dating_uk" },
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_dating_dev" }
                    ]
                },
                new(){
                    Name = "Publishing",
                    BackupEntries =
                    [
                        new(){ Type = BackupEntryType.MicrosoftSQL, Source = "takerman_publishing" }
                    ]
                },
                new () {
                    Name = "Proxy",
                    BackupEntries =
                    [
                        new(){ Type = BackupEntryType.Folder, Source = "nginx-proxy" },
                        new(){ Type = BackupEntryType.Folder, Source = "nginx-proxy-letsencrypt" }
                    ]
                }
            ];
        }

        public List<FileInfo> GetProjectPackages(string project)
        {
            var files = Directory.GetFiles(Path.Combine(_commonConfig.Value.BackupsLocation, project));

            var result = files.Select(x => new FileInfo(x)).ToList();

            return result;
        }

        public async Task MaintainBackups()
        {
            var packages = Directory.GetDirectories("/backups");
            foreach (var package in packages)
            {
                var backupFiles = new DirectoryInfo(package)
                    .GetFiles("*.zip")
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                var dailyBackups = backupFiles.Take(10).ToList();
                var monthlyBackups = backupFiles.GroupBy(x => x.CreationTime.Month).LastOrDefault();
                var yearlyBackups = backupFiles.GroupBy(x => x.CreationTime.Year).LastOrDefault();
                var backupsToKeep = dailyBackups;

                if (monthlyBackups != null && monthlyBackups.Any())
                {
                    backupFiles = [.. backupFiles, .. monthlyBackups.Take(3)];
                }

                if (yearlyBackups != null && yearlyBackups.Any())
                {
                    backupFiles = [.. backupFiles, .. yearlyBackups.Take(3)];
                }

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
                _logger.LogError(ex, $"**Backups Service**: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"**Backups Service**: {ex.Message}");
            }
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