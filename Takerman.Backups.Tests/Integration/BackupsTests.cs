using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class BackupsTests : TestBed<TestFixture>
    {
        private readonly IBackupsService? _backupsService;


        public BackupsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _backupsService = _fixture.GetService<IBackupsService>(_testOutputHelper);
        }

        [Fact]
        public void Should_BackupAllDatabase_When_ConnectedToTheServer()
        {
            var result = _backupsService.BackupAll(false);

            Assert.NotNull(result);
        }

        [Fact]
        public void Should_BackupDatabase_When_ConnectedToTheServer()
        {
            var result = _backupsService.Backup("takerman_dating_dev", false);

            Assert.NotNull(result);
        }

        [Fact]
        public void Should_GetAllBackups_When_ConnectedToTheServer()
        {
            var result = _backupsService.GetAll();

            Assert.NotNull(result);
        }
    }
}