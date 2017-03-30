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
		/// <param name="state">State.</param>
		public Dictionary<string, string> ExecuteAuthorizeOperation(ClaimsPrincipal user, OAuth2Client client, ITokenService tokenService)
		{
			// give the custom implementation a chance to adapt the claims
			var claims = tokenService.GetTokenClaims(user);

			// generate the signing credentails 
			var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(client.ClientSecret));
			var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			// define the expired time 
			var expires = DateTime.UtcNow.AddHours(24);

			// generate the JWT token 
			var token = new JwtSecurityToken(
				issuer: tokenService.GetTokenIssuer(),
				audience: tokenService.GetTokenIssuer(),
		  		claims: claims,
				notBefore: DateTime.UtcNow.AddMinutes(-15),
				expires: DateTime.UtcNow.AddHours(24),
				signingCredentials: credentials
			);

			// generate the encided token
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

			// generate the result
			var result = new Dictionary<string, string>();
			result.Add("token_type", "Bearer");
			result.Add("access_token", encodedJwt);
			result.Add("expires_in", Convert.ToInt64((expires - DateTime.UtcNow).TotalSeconds).ToString());

			// done
			return result;
		}
	}
}
