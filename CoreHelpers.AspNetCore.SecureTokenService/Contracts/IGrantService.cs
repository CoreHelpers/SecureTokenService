using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	internal interface IGrantService
	{
		Dictionary<string, string> ExecuteAuthorizeOperation(ClaimsPrincipal user, OAuth2Client client, ITokenService tokenService);
	}
}
