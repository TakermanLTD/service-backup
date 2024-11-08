using Takerman.Backups.Services;
using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class BackupsTests : TestBed<TestFixture>
    {
        private readonly ISqlService? _sqlService;

        public BackupsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _sqlService = _fixture.GetService<ISqlService>(_testOutputHelper);
        }

        [Fact(Skip = "It doesn't work entirely on locahost")]
        public async Task Should_BackupDailyDatabases_When_BackgroundServiceExecutes()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                var autoBackup = new AutoBackupService(_sqlService, null, null);

                await autoBackup.StartAsync(CancellationToken.None);
            });

            Assert.Null(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_BackupDatabase_When_ConnectedToTheServer()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                await _sqlService.BackupAsync("takerman_dating_dev");
            });

            Assert.NotNull(record?.Message);
        }

        [Fact(Skip = "Build")]
        public void Should_GetAllBackups_When_ConnectedToTheServer()
        {
            var result = _sqlService.GetBackups("master");

            Assert.NotNull(result);
        }

        [Fact(Skip = "Build")]
        public async Task Should_MaintainBackups_When_MaintenanceStarts()
        {
            var record = await Record.ExceptionAsync(_sqlService.MaintainBackups);

            Assert.Null(record?.Message);
        }
    }
}