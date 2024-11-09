namespace Takerman.Backups.Models.Configuration
{
    public class CommonConfig
    {
        public string BackupsLocation { get; set; } = string.Empty;

        public string MariaDBLocation { get; set; } = string.Empty;

        public string MicrosoftSqlLocation { get; set; } = string.Empty;

        public string VolumesLocation { get; set; } = string.Empty;
    }
}