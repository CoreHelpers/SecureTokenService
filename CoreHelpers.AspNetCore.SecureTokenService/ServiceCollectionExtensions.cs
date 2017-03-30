using System;
using Microsoft.Extensions.DependencyInjection;

namespace CoreHelpers.AspNetCore.SecureTokenService
{
    public static class ServiceCollectionExtensions
    {
		public static void AddSecureTokenService(this IServiceCollection services, Type tokenServiceType)
		{
			services.AddSingleton(typeof(ITokenService), tokenServiceType);
		}
    }
}
