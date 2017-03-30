using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class GrantCodeService : IGrantService
	{
		public Dictionary<string, string> ExecuteAuthorizeOperation(ClaimsPrincipal user, OAuth2Client client, ITokenService tokenService)
		{
			// generate a code 
			var code = Guid.NewGuid().ToString().Replace("-", "");

			// ask the token service to store this code in combination with this user and client request 
			tokenService.StoreCode(user, code);

			// handout the code
			var result = new Dictionary<string, string>();
			result.Add("code", code);
			return result;
		}

		public object ExecuteTokenOperation(OAuth2Client client, ITokenService tokenService, string optionalCode)
		{
			// consume the code 
			var user = tokenService.ConsumeCode(optionalCode);
			if (user == null) { return new { error = "invalid code", error_description = "the given code is invalid" }; };
				
			// give the custom implementation a chance to adapt the claims
			var claims = tokenService.GetTokenClaims(user);

			// issue the token 
			var issuedAccessToken = TokenIssueService.IssueToken(claims, tokenService.GetTokenIssuer(), tokenService.GetTokenAudience(), 24 * 60 * 60, client.ClientSecret);
			var issuedRefreshToken = TokenIssueService.IssueToken(claims, tokenService.GetTokenIssuer(), tokenService.GetTokenIssuer(), 6 * 30 * 24 * 60 * 60, client.ClientSecret);

			// done
			return new
			{
				access_token = issuedAccessToken.TokenValue,
				refresh_token = issuedRefreshToken.TokenValue,
				token_type = issuedAccessToken.TokenType,
				expires_in = Convert.ToInt64((issuedAccessToken.Expires - DateTime.UtcNow).TotalSeconds).ToString()
			};
		}
	}
}
