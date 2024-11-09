using Takerman.Backups.Services;
using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class BackupsTests : TestBed<TestFixture>
    {
        private readonly IPackagesService? _packagesService;

        public BackupsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _packagesService = _fixture.GetService<IPackagesService>(_testOutputHelper);
        }

        [Fact(Skip = "It doesn't work entirely on locahost")]
        public async Task Should_BackupDailyDatabases_When_BackgroundServiceExecutes()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                var autoBackup = new ScheduledBackgroundService(_packagesService, null, null);

                await autoBackup.StartAsync(CancellationToken.None);
            });

            Assert.Null(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_BackupDatabase_When_ConnectedToTheServer()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                await _packagesService.BackupDatabaseAsync("takerman_dating_dev");
            });

            Assert.NotNull(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_CreateBackupPackages_When_Requested()
        {
            var record = await Record.ExceptionAsync(_packagesService.CreateBackupPackages);

            Assert.Null(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_MaintainBackups_When_MaintenanceStarts()
        {
            var record = await Record.ExceptionAsync(_packagesService.MaintainBackups);

            Assert.Null(record?.Message);
        }
    }
}