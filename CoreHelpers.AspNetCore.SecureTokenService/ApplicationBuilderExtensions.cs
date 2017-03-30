using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseSecureTokenService(this IApplicationBuilder app, IRouteBuilder routeBuilder, string routePrefix = "sts")
		{
			routeBuilder.MapRoute(
				name: "oauth2_authorize",
				template: routePrefix + "/issue/oauth2/authorize",
  				defaults: new { controller = "OAuth", action = "Authorize" }
			);

			routeBuilder.MapRoute(
				name: "oauth2_token",
				template: routePrefix + "/issue/oauth2/token",
  				defaults: new { controller = "OAuth", action = "Token" }
			);

			return app;
		}
	}
}
