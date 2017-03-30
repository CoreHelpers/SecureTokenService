using System;
namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public class IssuedTokenModel
	{
		public string TokenType { get; set; }
		public string TokenValue { get; set; }
		public DateTime Expires { get; set; }
	}
}
