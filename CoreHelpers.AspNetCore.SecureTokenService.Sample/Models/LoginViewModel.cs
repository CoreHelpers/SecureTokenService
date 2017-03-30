using System;
using System.ComponentModel.DataAnnotations;

namespace CoreHelpers.AspNetCore.SecureTokenService.Sample.Models
{
	public class LoginViewModel
	{
		public string Username { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
		public string ReturnUrl { get; set; }
	}
}