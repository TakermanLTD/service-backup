using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takerman.Mail;
using Takerman.Backup.Data;
using Takerman.Backup.Data.Initialization;
using Takerman.Backup.Models.Configuration;
using Takerman.Backup.Services;
using Takerman.Backup.Services.Abstraction;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backup.Tests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
            => services
                .AddDbContext<DefaultContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Takerman.Backup.Data")))
                .AddTransient<DbContext, DefaultContext>()
                .AddTransient<IContextInitializer, ContextInitializer>()
                .AddTransient<ITemplateService, TemplateService>()
                .AddTransient<IMailService, MailService>()
                .Configure<RabbitMqConfig>(configuration.GetSection(nameof(RabbitMqConfig)))
                .Configure<TemplateConfig>(configuration.GetSection(nameof(TemplateConfig)));

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            var result = new List<TestAppSettings>()
            {
                new(){ Filename = "test-appsettings.json", IsOptional = false },
                new(){ Filename = "test-appsettings.Production.json", IsOptional = true }
            };

            return result;
        }
    }
}