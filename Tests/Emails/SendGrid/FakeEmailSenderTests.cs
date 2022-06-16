using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;
using SendGrid.Helpers.Mail;
using System;
using CSharpCommon.Emails.SendGrid;
using CSharpCommon.Tests.Shared.Unit;
using Xunit;

namespace CSharpCommon.Tests.Emails.SendGrid
{
	public class FakeEmailSenderTests : UnitTestBase
	{
		private readonly FakeEmailSender _instance;
		private readonly Mock<IHostEnvironment> _hostEnvironment;

		public FakeEmailSenderTests()
		{
			_hostEnvironment = MoqHelpers.GeneratePartialMock<IHostEnvironment>();
			_instance = new FakeEmailSender(_hostEnvironment.Object);
		}

		[Fact]
		public void SendEmailAsync_WHERE_environment_is_not_development_SHOULD_throw_error()
		{
			//arrange
			_hostEnvironment.Setup(x => x.EnvironmentName).Returns(EnvironmentName.Production);

			//act + assert
			_instance.Invoking(x => x.SendEmailAsync(It.IsAny<SendGridMessage>()))
					 .Should().Throw<Exception>()
					 .WithMessage("Do not use the fake email sender in a non-development environment.");
		}
	}
}
