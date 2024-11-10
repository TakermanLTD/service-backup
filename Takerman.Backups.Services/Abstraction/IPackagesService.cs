using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IPackagesService
    {
        Task BackupDatabaseAsync(string databaseName);

        Task CreateBackupPackage(string project);

        Task CreateBackupPackages();

        void DeletePackage(string package, string package1);
        
        List<ProjectDto> GetAll();
        
        List<PackageEntriesDto> GetProjectPackageEntries();

        List<FileInfo> GetProjectPackages(string project);

        Task MaintainBackups();

        Task OptimizeDatabaseAsync(string databaseName);
    }
}