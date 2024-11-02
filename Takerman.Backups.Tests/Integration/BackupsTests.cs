using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class BackupsTests : TestBed<TestFixture>
    {
        private readonly ISqlService? _backupsService;

        public BackupsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _backupsService = _fixture.GetService<ISqlService>(_testOutputHelper);
        }

        [Fact(Skip = "Build")]
        public async Task Should_BackupDatabase_When_ConnectedToTheServer()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                await _backupsService.BackupAsync("takerman_dating_dev");
            });

            Assert.NotNull(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_GetAllBackups_When_ConnectedToTheServer()
        {
            var result = await _backupsService.GetBackups("master");

            Assert.NotNull(result);
        }
    }
}