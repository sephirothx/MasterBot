using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace WarZone.Web
{
    public static class Extensions
    {
        public static IServiceCollection AddAPI<T>(this IServiceCollection services, string uri) where T : class
        {
            services.AddRefitClient<T>().ConfigureHttpClient(c => c.BaseAddress = new Uri(uri));

            return services;
        }
    }
}
