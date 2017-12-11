namespace FLM.Model.User
{
	public class UserInfo : IUserInfo
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public bool ShowAuditData => IsAdmin;

		public UserInfo()
		{
		}
	}
}