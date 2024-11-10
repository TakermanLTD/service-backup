namespace Takerman.Backups.Models.DTOs
{
    public class DashboardDto
    {
        public DateTime ScheduledFor { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 3, 0, 0);

        public double TotalSize { get; set; }
    }
}