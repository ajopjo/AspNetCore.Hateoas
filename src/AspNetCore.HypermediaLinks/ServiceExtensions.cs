using AspNetCore.HypermediaLinks.Configuration;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace AspNetCore.HypermediaLinks
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLinkBuilder(this IServiceCollection services, IConfiguration config = null)
        {
            if (config != null)
            {
                services.Configure<List<LinkConfiguration>>(o => config.GetSection("hypermediaLinks"));
            }

            services.TryAddScoped<IUrlHelperFactory, UrlHelperFactory>();
            services.TryAddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddScoped(x =>
            {
                return x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext);
            });

            return services;
        }
    }
}
