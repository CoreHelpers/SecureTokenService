using System;
using System.Security.Claims;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public interface ITokenService
	{
		string GetTokenIssuer();

		OAuth2Client FindClientById(string clientId);

		Claim[] GetTokenClaims(ClaimsPrincipal user);
	}
}
