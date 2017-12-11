using System.ComponentModel.DataAnnotations;

namespace FLM.Auth.IdentityServer.Models.AccountViewModels
{
	public class LoginWithRecoveryCodeViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Recovery Code")]
		public string RecoveryCode { get; set; }
	}
}