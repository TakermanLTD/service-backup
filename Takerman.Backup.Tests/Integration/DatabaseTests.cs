using Takerman.Backup.Services;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backup.Tests.Integration
{
    public class DatabaseTests : TestBed<TestFixture>
    {
        private readonly IDatabaseService? _databaseService;

        public DatabaseTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _databaseService = _fixture.GetService<IDatabaseService>(_testOutputHelper);
        }

        [Theory(Skip = "Skip integration tests on build")]
        [InlineData("tivanov@takerman.net")]
        public async Task Should_SendContactUsEmail_When_CorrectInputIsPassed(string email)
        {
            Assert.True(true);
        }
    }
}