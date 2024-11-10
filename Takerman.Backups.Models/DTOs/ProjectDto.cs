namespace Takerman.Backups.Models.DTOs
{
    public class ProjectDto
    {
        public string Name { get; set; } = string.Empty;

        public int PackagesCount { get; set; }

        public double TotalSizeMB { get; set; }
    }
}