using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class OAuthController : Controller
	{		
		[Authorize]
		[HttpGet]
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
				case "code":
					grantService = new GrantCodeService();
					break;
			}

			// check if we found a grant service
			if (grantService == null) { return new RedirectResult(redirect_uri.ToString() + "?server_error=Invalid or not supported grant type"); }

			// execute the grant operation 
			var grantResult = grantService.ExecuteAuthorizeOperation(User, oAuth2Client, tokenService);

			// redirect to the final destination
			return new RedirectResult(QueryHelpers.AddQueryString(redirect_uri.ToString(), grantResult));
		}
			
		[HttpPost]
		public IActionResult Token(OAuthTokenRequestModel requestModel, [FromServices] ITokenService tokenService)
		{
			// check if we have a grant_type
			if (requestModel.grant_type == null || requestModel.grant_type.Length == 0) { return new JsonResult(new { error = "grant missing", error_description = "value for grant_type is missing" }); }

			// read the client_id / client_secret from authentication header if exists and missing in the model
			string authHeader = Request.Headers["Authorization"];
			if (requestModel.client_id == null && authHeader != null && authHeader.StartsWith("Basic"))
			{
				string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
				Encoding encoding = Encoding.GetEncoding("iso-8859-1");
				string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
				int seperatorIndex = usernamePassword.IndexOf(':');
				requestModel.client_id = usernamePassword.Substring(0, seperatorIndex);
				requestModel.client_secret = usernamePassword.Substring(seperatorIndex + 1);
			}

			// validate our paramters
			if (requestModel.client_id == null || requestModel.client_id.Length == 0) { return new JsonResult(new { error = "client_id missing", error_description = "value for client_id is missing" }); }
			if (requestModel.client_secret == null || requestModel.client_secret.Length == 0) { return new JsonResult(new { error = "client_secret missing", error_description = "value for client_secret is missing" }); }

			// ask the token service implementation for the oAuth2 clinet
			var oAuth2Client = tokenService.FindClientById(requestModel.client_id);
			if (oAuth2Client == null) { return new JsonResult(new { error = "invalid client_id", error_description = "the given client_id is invalid" }); }

			// check if the secrets are the same
			if (!oAuth2Client.ClientSecret.Equals(requestModel.client_secret)) { return new JsonResult(new { error = "client_secret mismatch", error_description = "the provided client_secret does not fit" }); }

			// select the correct grantservice
			IGrantService grantService = null;
			switch (requestModel.grant_type)
			{
				case "authorization_code":
					grantService = new GrantCodeService();
					break;				
			}

			// check if we found a grant service
			if (grantService == null) { return new JsonResult(new { error = "not supported grant", error_description = "Didn't not found a supported grant service for " + requestModel.grant_type }); }

			// execute the grant operation 
			var grantResult = grantService.ExecuteTokenOperation(oAuth2Client, tokenService, requestModel.code);

			// done
			return new JsonResult(grantResult);
		}
	}
}
