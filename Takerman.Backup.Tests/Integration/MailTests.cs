using Takerman.Mail;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backup.Tests.Integration
{
    public class MailTests : TestBed<TestFixture>
    {
        private readonly IMailService _mailService;

        public MailTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _mailService = _fixture.GetService<IMailService>(_testOutputHelper);
        }

        [Theory(Skip = "Skip integration tests on build")]
        [InlineData("tivanov@takerman.net")]
        public async Task Should_SendContactUsEmail_When_CorrectInputIsPassed(string email)
        {
            var model = new MailMessageDto()
            {
                Body = $"From: Template Tests. <br />Email {email}. <br />Message: This is test email sent from the tests of Template",
                From = email,
                Subject = "New email from the contact form of Template website",
                To = "contact@takerman.net"
            };

            await Assert.ThrowsAsync<Exception>(async () => await _mailService.SendToQueue(model));
        }
    }
}