using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takerman.Backup.Services;
using Takerman.Mail;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backup.Tests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
            => services
                .AddTransient<IDatabaseService, DatabaseService>()
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