namespace FLM.Model.User
{
	public interface IUserInfo
	{
		string Id { get; set; }
		string Email { get; set; }
		bool IsAdmin { get; set; }
		bool ShowAuditData { get; }
	}
}