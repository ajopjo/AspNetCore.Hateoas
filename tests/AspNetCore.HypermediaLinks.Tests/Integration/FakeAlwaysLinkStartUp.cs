using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.HypermediaLinks;
using static Newtonsoft.Json.Formatting;
using static Newtonsoft.Json.NullValueHandling;
using static Newtonsoft.Json.StringEscapeHandling;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class FakeAlwaysLinkStartUp
    {
        public readonly IConfiguration Configuration;

        public FakeAlwaysLinkStartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().
                AddJsonFormatters().
                AddJsonOptions(op =>
                {
                    op.SerializerSettings.Formatting = Indented;
                    op.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    op.SerializerSettings.NullValueHandling = Ignore;                    
                    op.SerializerSettings.StringEscapeHandling = EscapeHtml;
                });
            services.AddLinkBuilder(Configuration, alwaysIncludeLinks: true);

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
