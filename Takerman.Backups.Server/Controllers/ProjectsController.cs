using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController(ILogger<ProjectsController> _logger, IPackagesService _projectsService) : ControllerBase
    {
        [HttpGet("Backup")]
        public async Task Backup(string project)
        {
            await _projectsService.CreateBackupPackage(project);
        }

        [HttpGet("BackupAll")]
        public async Task BackupAll()
        {
            await _projectsService.CreateBackupPackages();
        }

        [HttpGet("Delete")]
        public void Delete(string project, string package)
        {
            _projectsService.DeletePackage(project, package);
        }


        [HttpGet("GetAll")]
        public List<PackageEntriesDto> GetAll()
        {
            return _projectsService.GetProjectPackageEntries();
        }

        [HttpGet("GetPackages")]
        public List<FileInfo> GetPackages(string project)
        {
            return _projectsService.GetProjectPackages(project);
        }
    }
}