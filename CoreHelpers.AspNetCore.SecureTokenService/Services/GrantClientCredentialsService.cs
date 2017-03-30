using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class GrantClientCredentialsService : IGrantService
	{		
		public Dictionary<string, string> ExecuteAuthorizeOperation(ClaimsPrincipal user, OAuth2Client client, ITokenService tokenService)
		{
			throw new NotImplementedException();
		}

		public object ExecuteTokenOperation(OAuth2Client client, ITokenService tokenService, string optionalCode)
		{
			// load the associated user for client credentials flow
			var user = tokenService.GetClientCredentialsUser(client);
			if (user == null) { return new { error = "invalid principal", error_description = "Can't find the associated principal for client_credentials_flow" }; };

			// give the custom implementation a chance to adapt the claims
			var claims = tokenService.GetTokenClaims(user);

			// issue the token 
			var issuedAccessToken = TokenIssueService.IssueToken(claims, tokenService.GetTokenIssuer(), tokenService.GetTokenAudience(), 1 * 60 * 60, client.ClientSecret);

			// done
			return new
			{
				access_token = issuedAccessToken.TokenValue,
				token_type = issuedAccessToken.TokenType,
				expires_in = Convert.ToInt64((issuedAccessToken.Expires - DateTime.UtcNow).TotalSeconds).ToString()
			};
		}
	}
}
