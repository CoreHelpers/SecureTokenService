using System;
namespace CoreHelpers.AspNetCore.SecureTokenService
{
	internal enum nTokenServiceErrors
	{
		ErrorMissingRedirectUri		= 100001
	}


	internal class TokenServiceException : Exception
	{
		public TokenServiceException(nTokenServiceErrors code)
			: base(String.Format("Error Code: {0}", code))
		{
			
		}
	}
}
