using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FLM.Auth.IdentityServer
{
	public class Config
	{
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
			};
		}

		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("flm.api", "FLM API")
				{
					UserClaims =
					{
						JwtClaimTypes.Email
					},
					ApiSecrets =
					{
						new Secret("flm.api.s3cr3t".Sha256())
					}
				}
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
                // Angular JS Client
                new Client
				{
					ClientId = "flm.client.angular",
					ClientName = "FLM Angular Client",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,
					RequireConsent = false,

					RedirectUris = { "http://localhost:5001/authorize" },
					PostLogoutRedirectUris = { "http://localhost:5001" },
					AllowedCorsOrigins = { "http://localhost:5001" },

					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"flm.api"
					},
					AccessTokenLifetime = (int) TimeSpan.FromMinutes(60).TotalSeconds,
				}
			};
		}
	}
}