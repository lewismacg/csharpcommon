using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CSharpCommon.Emails.SendGrid.Interfaces
{
	public interface IEmailSender
	{
		Task SendEmailAsync(SendGridMessage message);
	}
}
