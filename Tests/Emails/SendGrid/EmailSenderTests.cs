using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;
using CSharpCommon.Emails.SendGrid;
using CSharpCommon.ErrorReporting.Raygun.Services;
using CSharpCommon.Tests.Shared.Unit;
using Xunit;

namespace CSharpCommon.Tests.Emails.SendGrid
{
	public class EmailSenderTests : UnitTestBase
	{
		private readonly Mock<ISendGridClient> _sendGridClient;
		private readonly Mock<IErrorReportingService> _errorReportingService;
		private readonly EmailSender _instance;

		public EmailSenderTests()
		{
			_sendGridClient = MoqHelpers.GenerateStrictMock<ISendGridClient>();
			_errorReportingService = MoqHelpers.GeneratePartialMock<IErrorReportingService>();

			_instance = new EmailSender(_sendGridClient.Object, _errorReportingService.Object);
		}

		#region SendEmailAsync

		[Fact]
		public async Task SendEmailAsync_WHERE_response_is_not_success_SHOULD_report_error()
		{
			//arrange
			const string templateId = "temple templeton";
			var message = new SendGridMessage { TemplateId = templateId };

			var response = MoqHelpers.GenerateStrictMock<Response>(HttpStatusCode.Forbidden, null, null);
			_sendGridClient.Setup(x => x.SendEmailAsync(message, default)).ReturnsAsync(response.Object);

			//act
			await _instance.SendEmailAsync(message);

			//assert
			_errorReportingService.Verify(x => x.ReportErrorAsync(It.Is<Exception>(y => y.Message == $"Failed to send email with template ID {templateId}")), Times.Once);
		}

		[Fact]
		public async Task SendEmailAsync()
		{
			//arrange
			var message = MoqHelpers.GenerateStrictMock<SendGridMessage>();

			var response = MoqHelpers.GenerateStrictMock<Response>(HttpStatusCode.OK, null, null);
			_sendGridClient.Setup(x => x.SendEmailAsync(message.Object, default)).ReturnsAsync(response.Object);

			//act
			await _instance.SendEmailAsync(message.Object);

			//assert
			_errorReportingService.Verify(x => x.ReportErrorAsync(It.IsAny<Exception>()), Times.Never);
		}

		#endregion
	}
}
