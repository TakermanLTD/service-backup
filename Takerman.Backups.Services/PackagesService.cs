using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
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
        public async Task BackupDatabaseAsync(string databaseName, string destination, BackupEntryType type)
        {
            try
            {
                if (await IsDatabaseExisting(databaseName, type))
                {
                    switch (type)
                    {
                        case BackupEntryType.MicrosoftSQL:
                            var query = @$"
                            DECLARE @BackupFileName NVARCHAR(255);
                            SET @BackupFileName = @FileName;
                            BACKUP DATABASE {databaseName} TO DISK = @BackupFileName;";

                            var fileName = Path.Combine(destination, $"{databaseName}.bak");

                            using (var connection = new SqlConnection(_connectionString.Value.MicrosoftSqlConnection))
                            {
                                await connection.OpenAsync();
                                await using var command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@FileName", fileName);
                                await command.ExecuteNonQueryAsync();
                            }
                            break;

                        case BackupEntryType.MySQL:
                        case BackupEntryType.MariaDB:
                            using (var conn = new MySqlConnection(_connectionString.Value.MySqlConnection))
                            {
                                using var cmd = new MySqlCommand($"mysqldump -u root --port=5306 -p printing > {Path.Combine(destination, "test.sql")}");
                                cmd.Connection = conn;
                                conn.Open();
                                await cmd.ExecuteNonQueryAsync();
                                conn.Close();
                            }
                            break;

                        case BackupEntryType.SQLite:
                            break;
                    }
                }
                else
                {
                    _logger.LogWarning("*Backups Service*: Tried to backup a microsoft database for a package, but the database does not exits");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.GetMessage());
            }
        }

        public async Task CreateBackupPackage(string project, string prefix = "manual")
        {
            var packages = GetProjectPackageEntries();
            var package = packages.FirstOrDefault(x => x.Name.ToLower() == project.ToLower());
            var packageName = $"{package.Name}_{DateTime.Now:yyyy_MM_dd_hh_mm}";
            if (!string.IsNullOrEmpty(prefix))
                packageName = prefix + "_" + packageName;
            var packageDirectory = Path.Combine(_commonConfig.Value.BackupsLocation, package.Name, packageName);

            if (!Directory.Exists(packageDirectory))
                Directory.CreateDirectory(packageDirectory);

            foreach (var entry in package.BackupEntries)
            {
                switch (entry.Type)
                {
                    case BackupEntryType.MicrosoftSQL:
                        await BackupDatabaseAsync(entry.Source, packageDirectory, entry.Type);
                        break;

                    case BackupEntryType.MySQL:
                    case BackupEntryType.MariaDB:
                        await BackupDatabaseAsync(entry.Source, packageDirectory, entry.Type);
                        break;

                    case BackupEntryType.SQLite:
                        break;

                    case BackupEntryType.Folder:
                        var sourceDirectory = Path.Combine(_commonConfig.Value.VolumesLocation, entry.Source);
                        if (Directory.Exists(sourceDirectory))
                        {
                            new DirectoryInfo(sourceDirectory).CopyFolder(Path.Combine(packageDirectory, entry.Prefix + entry.Source + "_" + packageName));
                        }
                        else
                        {
                            _logger.LogWarning("*Backups Service*: Tried to copy files for a package, but the files does not exits");
                        }
                        break;

                    case BackupEntryType.File:
                        var filePath = Path.Combine(_commonConfig.Value.VolumesLocation, entry.Source);
                        if (File.Exists(filePath))
                        {
                            File.Copy(Path.Combine(_commonConfig.Value.VolumesLocation, entry.Source), packageDirectory);
                        }
                        else
                        {
                            _logger.LogWarning("*Backups Service*: Tried to copy files for a package, but the files does not exits");
                        }
                        break;
                }
            }

            ZipFile.CreateFromDirectory(packageDirectory, packageDirectory + ".zip", CompressionLevel.Optimal, true);

            Directory.Delete(packageDirectory, true);
        }

        public async Task CreateBackupPackages(string prefix = "manual")
        {
            var packages = GetProjectPackageEntries();

            foreach (var package in packages)
            {
                await CreateBackupPackage(package.Name, prefix);
            }
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
                        Name = new DirectoryInfo(dir).Name,
                        PackagesCount = files.Length,
                        TotalSizeMB = (files.Sum(x => new FileInfo(x).Length) / 1024) / 1024D
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
                        new(){ Type = BackupEntryType.Folder, Source = Path.Combine("mariadb", "printing"), Prefix = "database_" },
                        new(){ Type = BackupEntryType.Folder, Source = "printing", Prefix = "website_" }
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

        public async Task<List<PackageDto>> GetProjectPackages(string project)
        {
            var files = Directory.GetFiles(Path.Combine(_commonConfig.Value.BackupsLocation, project));

            var fileInfos = files.Select(x => new FileInfo(x)).ToList();

            var result = fileInfos.Select(x => new PackageDto()
            {
                Created = x.CreationTime,
                Name = x.Name,
                Size = (x.Length / 1024) / 1024D
            }).ToList();

            return result;
        }

        public async Task<bool> IsDatabaseExisting(string database, BackupEntryType type)
        {
            var query = string.Empty;
            switch (type)
            {
                case BackupEntryType.MicrosoftSQL:
                    query = $@"SELECT * FROM master.sys.databases WHERE name = N'{database}'";
                    break;

                case BackupEntryType.MySQL:
                case BackupEntryType.MariaDB:
                    break;

                case BackupEntryType.SQLite:
                    break;
            }

            if (string.IsNullOrEmpty(query))
                return true;

            var result = await Select<dynamic>(query);

            return result?.Count > 0;
        }

        public void MaintainBackups()
        {
            var projects = Directory.GetDirectories(_commonConfig.Value.BackupsLocation);
            foreach (var project in projects)
            {
                var backupFiles = new DirectoryInfo(project)
                    .GetFiles("*.zip")
                    .OrderByDescending(f => f.CreationTime)
                    .ToList();

                var dailyBackups = backupFiles.Take(10).ToList();
                var monthlyBackups = backupFiles.GroupBy(x => x.CreationTime.Month).FirstOrDefault();
                var yearlyBackups = backupFiles.GroupBy(x => x.CreationTime.Year).FirstOrDefault();
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
                await using var connection = new SqlConnection(_connectionString.Value.MicrosoftSqlConnection);
                await connection.OpenAsync();

                await using var command = new SqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"*Backups Service*: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"*Backups Service*: {ex.Message}");
            }
        }

        public async Task<List<T>> Select<T>(string query) where T : new()
        {
            await using var connection = new SqlConnection(_connectionString.Value.MicrosoftSqlConnection);
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