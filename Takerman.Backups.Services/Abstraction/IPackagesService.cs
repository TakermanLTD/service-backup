using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IPackagesService
    {
        Task BackupDatabaseAsync(string databaseName, string destination, BackupEntryType type);

        Task CreateBackupPackage(string project, string prefix = "manual");

        Task CreateBackupPackages(string prefix = "manual");

        void DeletePackage(string package, string package1);

        List<ProjectDto> GetAll();

        List<PackageEntriesDto> GetProjectPackageEntries();

        Task<List<PackageDto>> GetProjectPackages(string project);

        void MaintainBackups();

        Task OptimizeDatabaseAsync(string databaseName);

        Task<bool> IsDatabaseExisting(string database, BackupEntryType type);
    }
}