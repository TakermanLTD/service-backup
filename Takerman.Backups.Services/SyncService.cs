using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Services.Abstraction;

namespace Takerman.Backups.Services
{
    public class SyncService(ILogger<SyncService> _logger, IOptions<GoogleDriveConfig> _gdConfig) : ISyncService
    {
        private DriveService _driveService;

        public async Task<string> UploadFileAsync(string path)
        {
            var clientId = _gdConfig.Value.ClientId;
            var clientSecret = _gdConfig.Value.ClientSecret;
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret },
                [DriveService.Scope.DriveFile],
                "user",
                CancellationToken.None);
            _driveService = new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credential, ApplicationName = "YourAppName" });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File { Name = Path.GetFileName(path) };
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                request = _driveService.Files.Create(fileMetadata, stream, "application/octet-stream");
                request.Fields = "id";
                await request.UploadAsync();
            }

            var file = request.ResponseBody;
            return file.Id;
        }
    }
}