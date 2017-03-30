using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreHelpers.AspNetCore.SecureTokenService.Sample.Models;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CoreHelpers.AspNetCore.SecureTokenService.Sample.Controllers
{
	public class SessionController : Controller
	{
		[HttpGet]
		public IActionResult Login(string returnUrl = "")
		{
			var model = new LoginViewModel { ReturnUrl = returnUrl };
			return View(model);		
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			Console.WriteLine(User.ToString());

			if (ModelState.IsValid)
			{
				var claimsIdentity = new ClaimsIdentity(null, "AuthenticationCookie");
				claimsIdentity.AddClaim(new Claim("iss", "sts.xxx.com"));


				await HttpContext.Authentication.SignInAsync(
	  				"AuthenticationCookie",
					new ClaimsPrincipal(claimsIdentity),
	   				new AuthenticationProperties
	   				{
							IsPersistent = model.RememberMe,
							ExpiresUtc = DateTime.UtcNow.AddMinutes(20)					                           
	   				});

					return Redirect(model.ReturnUrl);					
			}
			ModelState.AddModelError("", "Invalid login attempt");
			return View(model);
		}
	}
}
