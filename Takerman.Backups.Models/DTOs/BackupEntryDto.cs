namespace Takerman.Backups.Models.DTOs
{
    public class BackupEntryDto
    {
        public string Source { get; set; }

        public BackupEntryType Type { get; set; }

        public string Prefix { get; set; }
    }
}