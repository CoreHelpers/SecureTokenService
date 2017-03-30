using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class AuthorizeController : Controller
	{		
		[Authorize]
		public IActionResult Authorize([FromQuery]string client_id, [FromQuery]string response_type, [FromQuery] Uri redirect_uri, [FromServices] ITokenService tokenService)
		{
			// check if we have a redirect_uri
			if (redirect_uri == null) { throw new TokenServiceException(nTokenServiceErrors.ErrorMissingRedirectUri); }

			// validate our paramters
			if (client_id == null || client_id.Length == 0) { return new RedirectResult(redirect_uri.ToString() + "?server_error=Missing client_id"); }
			if (response_type == null || response_type.Length == 0) { return new RedirectResult(redirect_uri.ToString() + "?server_error=Missing response_type"); }


			// ask the token service implementation for the oAuth2 clinet
			var oAuth2Client = tokenService.FindClientById(client_id);
			if (oAuth2Client == null) { return new RedirectResult(redirect_uri.ToString() + "?server_error=Invalid client_id"); }

			// verify redirectURI
			if (!oAuth2Client.RedirectUris.Contains(redirect_uri)) { return new RedirectResult(redirect_uri.ToString() + "?server_error=RedirectUri mismatch"); }

			// fullfill the requested grant
			IGrantService grantService = null;
			switch (response_type)
			{
				case "token":
					grantService = new GrantTokenService();
					break;				
			}

			// check if we found a grant server
			if (grantService == null) { return new RedirectResult(redirect_uri.ToString() + "?server_error=Invalid or not supported grant type"); }

			// execute the grant operation 
			var grantResult = grantService.ExecuteAuthorizeOperation(User, oAuth2Client, tokenService);

			// redirect to the final destination
			return new RedirectResult(QueryHelpers.AddQueryString(redirect_uri.ToString(), grantResult));
		}
	}
}
