
namespace Takerman.Backups.Services.Abstraction
{
    public interface ISyncService
    {
        Task<string> UploadFileAsync(string path);
    }
}