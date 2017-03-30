using System;
namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class OAuthTokenRequestModel
	{
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string grant_type { get; set; }
		public string code { get; set; }
	}
}
