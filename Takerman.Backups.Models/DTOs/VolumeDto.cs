namespace Takerman.Backups.Models.DTOs
{
    public class VolumeDto
    {
        public DateTime Created { get; set; } = DateTime.Now;

        public string Name { get; set; } = string.Empty;

        public double Size { get; set; }
    }
}