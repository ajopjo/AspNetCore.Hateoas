using AspNetCore.HypermediaLinks.Configuration;
using AspNetCore.HypermediaLinks.Formatter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace AspNetCore.HypermediaLinks
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLinkBuilder(this IServiceCollection services, IConfiguration config = null, bool alwaysIncludeLinks = false)
        {
            if (config != null)
            {
                services.Configure<List<LinkConfiguration>>(o => config.GetSection("hypermediaLinks"));
            }

            services.TryAddSingleton(new LinkBuilderSettings() { AlwaysIncludeLinks = alwaysIncludeLinks });
            services.TryAddScoped<IUrlHelperFactory, UrlHelperFactory>();
            services.TryAddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddScoped(x =>
            {
                return x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext);
            });
            services.AddSingleton<IConfigureOptions<MvcOptions>, HyperMediaJsonSettingsOptions>();
            return services;
        }
    }
}
