using System;
using System.Collections.Generic;
using System.Security.Claims;
using CoreHelpers.AspNetCore.SecureTokenService;

namespace CoreHelpers.AspNetCore.SecureTokenService.Sample
{
	public class TokenServiceImpl : ITokenService
	{
		public OAuth2Client FindClientById(string clientId)
		{
			return new OAuth2Client()
			{
				ClientId = clientId,
				ClientSecret = "60E48ECA-E706-4443-A74C-D9D8CF83850D",
				RedirectUris = new List<Uri>() { new Uri("http://localhost/") }
			};
		}

		public Claim[] GetTokenClaims(ClaimsPrincipal user)
		{
			var claims = new List<Claim>();

			claims.Add(new Claim("name", "DemoUser"));

			return claims.ToArray();
		}

		public string GetTokenIssuer()
		{
			return "sts.acme.org";
		}
	}
}
