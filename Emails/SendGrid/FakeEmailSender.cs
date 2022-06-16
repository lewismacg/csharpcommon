using Microsoft.Extensions.Hosting;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using CSharpCommon.Emails.SendGrid.Interfaces;

namespace CSharpCommon.Emails.SendGrid
{
	public class FakeEmailSender : IEmailSender
	{
		private readonly IHostEnvironment _hostEnvironment;

		public FakeEmailSender(IHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
		}

		public async Task SendEmailAsync(SendGridMessage message)
		{
			if (!_hostEnvironment.IsDevelopment()) throw new Exception("Do not use the fake email sender in a non-development environment.");

			var webRootPath = $"{_hostEnvironment.ContentRootPath}\\testemailbody.txt";

			await using var writer = System.IO.File.CreateText(webRootPath);
			await writer.WriteAsync(message.PlainTextContent);
		}
	}
}
