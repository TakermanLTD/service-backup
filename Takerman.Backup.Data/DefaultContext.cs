using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Takerman.Backup.Data
{
    public class DefaultContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TemplateEntity> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}