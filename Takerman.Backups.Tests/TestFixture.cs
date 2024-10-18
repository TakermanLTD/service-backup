using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Services.Abstraction;
using Takerman.Backupss.Services;
using Takerman.Mail;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
            => services
                .Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)))
                .AddTransient<IDatabasesService, DatabasesService>()
                .AddTransient<IBackupsService, BackupsService>()
                .AddTransient<IMailService, MailService>();

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