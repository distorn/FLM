using FLM.Model.User;

namespace FLM.DAL.Contracts
{
	public interface IUserResolver
	{
		IUserInfo GetUser();
	}
}