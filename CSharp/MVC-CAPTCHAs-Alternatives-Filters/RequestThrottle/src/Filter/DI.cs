
namespace RequestThrottle.Filter
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDefaultRequestThrottleService(this IServiceCollection services)
        {

            services.AddSingleton<IRequestThrottleService, RequestThrottleService>();
            services.AddSingleton<IRequestThrottleFilterAttribute, RequestThrottleFilterAttribute>();
            return services;
        }
    }
}