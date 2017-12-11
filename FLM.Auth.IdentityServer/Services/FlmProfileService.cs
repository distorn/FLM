using FLM.Auth.IdentityServer.Models;
using FLM.Model.User;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FLM.Auth.IdentityServer.Services
{
	public class FlmProfileService : IProfileService
	{
		private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
		private readonly UserManager<ApplicationUser> _userManager;

		public FlmProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
		}

		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var sub = context.Subject.GetSubjectId();

			var user = await _userManager.FindByIdAsync(sub);
			var principal = await _claimsFactory.CreateAsync(user);

			var claims = principal.Claims.ToList();

			claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

			if (principal.IsInRole(UserRoles.Administrator))
			{
				claims.Add(new Claim(JwtClaimTypes.Role, UserRoles.Administrator));
			}
			else
			{
				claims.Add(new Claim(JwtClaimTypes.Role, UserRoles.User));
			}

			context.IssuedClaims = claims;
		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var sub = context.Subject.GetSubjectId();
			var user = await _userManager.FindByIdAsync(sub);
			context.IsActive = user != null;
		}
	}
}