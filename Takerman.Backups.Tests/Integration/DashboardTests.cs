using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class DashboardTests : TestBed<TestFixture>
    {
        private readonly IDashboardService? _dashboardService;
        private readonly IPackagesService? _packagesService;

        public DashboardTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _dashboardService = _fixture.GetService<IDashboardService>(_testOutputHelper);
            _packagesService = _fixture.GetService<IPackagesService>(_testOutputHelper);
        }

        [Fact(Skip = "It doesn't work entirely on locahost")]
        public async Task Should_BackupDailyDatabases_When_BackgroundServiceExecutes()
        {
        }

        [Fact]
        public async Task Should_GetAllProjects_When_ARequestHasBeenMaid()
        {
            var actual = _packagesService.GetAll();

            Assert.NotNull(actual);
        }
    }
}