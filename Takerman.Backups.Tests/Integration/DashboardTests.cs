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

        [Fact(Skip = "Build")]
        public async Task Should_GetDashboardData_When_TheMethodIsCalled()
        {
            var data = _dashboardService.GetDashboard();

            Assert.NotNull(data);
        }

        [Fact(Skip = "Build")]
        public async Task Should_GetAllProjects_When_ARequestHasBeenMaid()
        {
            var actual = _packagesService.GetAll();

            Assert.NotNull(actual);
        }
    }
}