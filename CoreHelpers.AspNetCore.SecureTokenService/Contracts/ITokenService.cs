using System;
using System.Security.Claims;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public interface ITokenService
	{
		string GetTokenIssuer();

		string GetTokenAudience();

		OAuth2Client FindClientById(string clientId);

		Claim[] GetTokenClaims(ClaimsPrincipal user);

		void StoreCode(ClaimsPrincipal user, string code);

		ClaimsPrincipal ConsumeCode(string code);

		ClaimsPrincipal GetClientCredentialsUser(OAuth2Client client);
	}
}
