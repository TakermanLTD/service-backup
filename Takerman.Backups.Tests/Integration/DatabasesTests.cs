using Takerman.Backups.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class DatabasesTests : TestBed<TestFixture>
    {
        private readonly ISqlService? _sqlService;

        public DatabasesTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _sqlService = _fixture.GetService<ISqlService>(_testOutputHelper);
        }

        [Fact(Skip = "Build")]
        public async void Should_CreateDatabase_When_ConnectedToTheServer()
        {
            var record = await Record.ExceptionAsync(async () =>
            {
                await _sqlService.CreateDatabaseAsync("takerman_test");
            });

            Assert.Null(record?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_DeleteDatabase_When_ConnectedToTheServer()
        {
            var created = await Record.ExceptionAsync(async () =>
            {
                await _sqlService.CreateDatabaseAsync("takerman_test");
            });
            Assert.Null(created?.Message);

            var deleted = await Record.ExceptionAsync(async () =>
            {
                await _sqlService.DropDatabaseAsync("takerman_test");
            });

            Assert.Null(deleted?.Message);
        }

        [Fact(Skip = "Build")]
        public async Task Should_GetAllDatabases_When_ConnectedToTheServer()
        {
            var result = await _sqlService.GetAllDatabasesAsync();

            Assert.NotNull(result);
        }
    }
}