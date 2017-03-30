using System;
using System.Collections.Generic;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class OAuth2Client
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public List<Uri> RedirectUris { get; set; }
	}
}
