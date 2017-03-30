using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	internal class GrantTokenService : IGrantService
	{
		/// <summary>
		/// We need to generate a token in this flow so just issue the JWT token for the given user 
		/// </summary>
		/// <returns>The authorize operation.</returns>
		/// <param name="user">User.</param>
		/// <param name="client">Client.</param>
		/// <param name="tokenService">TokenService.</param>
		public Dictionary<string, string> ExecuteAuthorizeOperation(ClaimsPrincipal user, OAuth2Client client, ITokenService tokenService)
		{
			// give the custom implementation a chance to adapt the claims
			var claims = tokenService.GetTokenClaims(user);

			// issue the token 
			var issuedToken = TokenIssueService.IssueToken(claims, tokenService.GetTokenIssuer(), tokenService.GetTokenIssuer(), 24 * 60 * 60, client.ClientSecret);

			// generate the result
			var result = new Dictionary<string, string>();
			result.Add("token_type", issuedToken.TokenType);
			result.Add("access_token", issuedToken.TokenValue);
			result.Add("expires_in", Convert.ToInt64((issuedToken.Expires - DateTime.UtcNow).TotalSeconds).ToString());

			// done
			return result;
		}

		public object ExecuteTokenOperation(OAuth2Client client, ITokenService tokenService, string optionalCode)
		{
			throw new NotImplementedException();
		}
	}
}
