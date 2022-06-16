using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using CSharpCommon.Emails.SendGrid.Interfaces;
using CSharpCommon.ErrorReporting.Raygun.Services;

namespace CSharpCommon.Emails.SendGrid
{
	public class EmailSender : IEmailSender
	{
		private readonly ISendGridClient _client;
		private readonly IErrorReportingService _errorReportingService;

		public EmailSender(ISendGridClient client, IErrorReportingService errorReportingService)
		{
			_errorReportingService = errorReportingService;
			_client = client;
		}

		public async Task SendEmailAsync(SendGridMessage message)
		{
			var response = await _client.SendEmailAsync(message);

			if (!response.IsSuccessStatusCode) await _errorReportingService.ReportErrorAsync(new Exception($"Failed to send email with template ID {message.TemplateId}"));
		}
	}
}
