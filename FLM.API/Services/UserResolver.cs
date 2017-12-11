using FLM.DAL.Contracts;
using FLM.Model.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FLM.API.Services
{
	public class UserResolver : IUserResolver
	{
		private const string CLAIM_ID = "sub";
		private const string CLAIM_EMAIL = "email";

		private readonly IHttpContextAccessor _context;

		public UserResolver(IHttpContextAccessor context)
		{
			_context = context;
		}

		public IUserInfo GetUser()
		{
			var identity = (ClaimsIdentity)_context.HttpContext.User?.Identity;
			if (identity != null && identity.IsAuthenticated)
			{
				return new UserInfo()
				{
					Id = identity.FindFirst(CLAIM_ID).Value,
					Email = identity.FindFirst(CLAIM_EMAIL).Value,
					IsAdmin = identity.FindFirst(identity.RoleClaimType).Value == UserRoles.Administrator
				};
			}

			return null;
		}
	}
}