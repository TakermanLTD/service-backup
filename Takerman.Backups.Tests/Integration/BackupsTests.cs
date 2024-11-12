using Microsoft.Extensions.Configuration;
using Takerman.Backups.Models.Configuration;
using Takerman.Backups.Models.DTOs;
using Takerman.Backups.Services;
using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class BackupsTests : TestBed<TestFixture>
    {
        private readonly IPackagesService? _packagesService;
        private readonly ISyncService? _syncService;
        private readonly CommonConfig _commonConfig;

        public BackupsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _packagesService = _fixture.GetService<IPackagesService>(_testOutputHelper);
            _syncService = _fixture.GetService<ISyncService>(_testOutputHelper);
            _commonConfig = _fixture.Configuration.GetSection(nameof(CommonConfig)).Get<CommonConfig>();
        }

        [Fact(Skip = "Build")]
        public async Task Should_BackupDailyDatabases_When_BackgroundServiceExecutes()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                var autoBackup = new ScheduledBackgroundService(_packagesService, _syncService, null, null);

                await autoBackup.StartAsync(CancellationToken.None);
            });

            Assert.Null(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_BackupDatabase_When_ConnectedToTheServer()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                await _packagesService.BackupDatabaseAsync("takerman_dating_dev", Path.Combine(_commonConfig.BackupsLocation, "tests"), BackupEntryType.MicrosoftSQL);
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