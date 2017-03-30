using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class TokenIssueService
	{
		public static IssuedTokenModel IssueToken(Claim[] claims, string tokenIssuer, string tokenAudience, Int64 ttl, string signingSecret)
		{
			// generate the signing credentails 
			var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingSecret));
			var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

			// define the expired time 
			var expires = DateTime.UtcNow.AddSeconds(ttl);

			// generate the JWT token 
			var token = new JwtSecurityToken(
				issuer: tokenIssuer,
				audience: tokenAudience,
		  		claims: claims,
				notBefore: DateTime.UtcNow.AddMinutes(-15),
				expires: expires,
				signingCredentials: credentials
			);

			// generate the encided token
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

			// generate the summary
			return new IssuedTokenModel() {
				TokenType = "Bearer",
				TokenValue = encodedJwt,
				Expires = expires
			};
		}
	}
}
