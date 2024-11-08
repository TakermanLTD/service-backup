using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class VolumesTests : TestBed<TestFixture>
    {
        private readonly IVolumesService? _volumesService;

        public VolumesTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _volumesService = _fixture.GetService<IVolumesService>(_testOutputHelper);
        }

        [Fact(Skip = "Build")]
        public void Should_BackupVolumes_When_BackupJobStarts()
        {
            var record = Record.Exception(_volumesService.BackupAsync);

            Assert.Null(record?.Message);
        }
    }
}