namespace Takerman.Backups.Models.DTOs
{
    public class PackageEntriesDto
    {
        public List<BackupEntryDto> BackupEntries { get; set; } = [];

        public string Name { get; set; } = string.Empty;

        public double Size { get; set; }
    }
}