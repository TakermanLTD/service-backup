namespace Takerman.Backups.Models.DTOs
{
    public class PackageDto
    {
        public DateTime Created { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Size { get; set; }
    }
}