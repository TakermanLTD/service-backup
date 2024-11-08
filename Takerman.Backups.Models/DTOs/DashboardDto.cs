namespace Takerman.Backups.Models.DTOs
{
    public class DashboardDto
    {
        public string BackupsLocation { get; set; } = string.Empty;

        public double BackupsSize { get; set; }

        public string DatabasesLocation { get; set; } = string.Empty;

        public double DatabasesSize { get; set; }

        public string VolumesLocation { get; set; } = string.Empty;

        public double VolumesSize { get; set; }
    }
}