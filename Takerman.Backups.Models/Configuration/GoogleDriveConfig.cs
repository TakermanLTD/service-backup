namespace Takerman.Backups.Models.Configuration
{
    public class GoogleDriveConfig
    {
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;

        public string RedicrectUri { get; set; } = string.Empty;
    }
}