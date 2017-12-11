using System.Threading.Tasks;

namespace FLM.Auth.IdentityServer.Services
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}
}