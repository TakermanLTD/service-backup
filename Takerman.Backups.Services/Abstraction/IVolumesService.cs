using Takerman.Backups.Models.DTOs;

namespace Takerman.Backups.Services.Abstraction
{
    public interface IVolumesService
    {
        void BackupAsync();

        List<VolumeDto> GetAll();

        Task MaintainAsync();

        void Remove(string volume);
    }
}