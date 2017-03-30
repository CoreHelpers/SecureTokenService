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
  				defaults: new { controller = "Authorize", action = "Authorize" }
			);

			return app;
		}
	}
}
