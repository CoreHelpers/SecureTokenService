using System;
using System.Collections.Generic;
using System.Security.Claims;
using CoreHelpers.AspNetCore.SecureTokenService;

namespace CoreHelpers.AspNetCore.SecureTokenService.Sample
{
	public class TokenServiceImpl : ITokenService
	{
		private Dictionary<string, ClaimsPrincipal> _codeCache = new Dictionary<string, ClaimsPrincipal>();

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

		public string GetTokenAudience() {
			return "api.acme.org";
		}
			
		public void StoreCode(ClaimsPrincipal user, string code)
		{
			_codeCache.Add(code, user);	
		}

		public ClaimsPrincipal ConsumeCode(string code)
		{
			if (_codeCache.ContainsKey(code))
				return _codeCache[code];
			else
				return null;
		}

		public ClaimsPrincipal GetClientCredentialsUser(OAuth2Client client)
		{
			var identity = new ClaimsIdentity();
			identity.AddClaim(new Claim("ccf", "1"));
			                  
			var principal = new ClaimsPrincipal();
			principal.AddIdentity(identity);

			return principal;
      }
	}
}
