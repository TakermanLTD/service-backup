using Microsoft.AspNetCore.Mvc;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VolumesController(ILogger<VolumesController> _logger, IVolumesService _volumesService) : ControllerBase
    {
        [HttpGet("GetAll")]
        public List<VolumeDto> GetAll()
        {
            return _volumesService.GetAll();
        }

        [HttpGet("Backup")]
        public void Backup()
        {
            _volumesService.BackupAsync();
        }

        [HttpGet("Maintain")]
        public async Task Maintain()
        {
            await _volumesService.MaintainAsync();
        }

        [HttpGet("Remove")]
        public void Remove(string volume)
        {
            _volumesService.Remove(volume);
        }
    }
}