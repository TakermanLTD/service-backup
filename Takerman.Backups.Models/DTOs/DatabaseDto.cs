namespace Takerman.Backups.Models.DTOs
{
    public class DatabaseDto
    {
        public decimal DataSizeMB { get; set; }

        public decimal LogSizeMB { get; set; }

        public string Name { get; set; } = string.Empty;

        public string RecoveryModel { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}