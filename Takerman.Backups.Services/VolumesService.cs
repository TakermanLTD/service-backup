using Microsoft.Extensions.Options;
using System.IO.Compression;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class VolumesService(IOptions<CommonConfig> _commonConfig) : IVolumesService
    {
        public string VolumesLocation
        {
            get
            {
                var result = Path.Combine(_commonConfig.Value.HomeLocation, "volumes");

                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);

                return result;
            }
        }

        public string VolumesBackupsLocation
        {
            get
            {
                var result = Path.Combine(_commonConfig.Value.HomeLocation, "backups", "volumes");

                if (!Directory.Exists(result))
                    Directory.CreateDirectory(result);

                return result;
            }
        }

        public void BackupAsync()
        {
            var backupLocation = Path.Combine(VolumesBackupsLocation, $"volumes_{DateTime.Now:yy_MM_dd_HH_mm}.zip");
            
            ZipFile.CreateFromDirectory(VolumesLocation, backupLocation, CompressionLevel.Optimal, true);
        }

        public List<VolumeDto> GetAll()
        {
            return new List<VolumeDto>();
        }

        public Task MaintainAsync()
        {
            throw new NotImplementedException();
        }

        public void Remove(string volume)
        {
            File.Delete(Path.Combine(VolumesBackupsLocation, volume));
        }
    }
}