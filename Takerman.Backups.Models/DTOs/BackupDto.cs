
namespace Takerman.Backups.Models.DTOs
{
    public class BackupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
        
        public decimal Size { get; set; }
    }
}