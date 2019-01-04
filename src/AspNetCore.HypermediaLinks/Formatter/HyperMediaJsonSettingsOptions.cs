using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Formatter
{
    public class HyperMediaJsonSettingsOptions : IConfigureOptions<MvcOptions>
    {
        private readonly MvcJsonOptions _options;
        private readonly ArrayPool<char> _arrayPool;
        private readonly LinkBuilderSettings _settings;

        public HyperMediaJsonSettingsOptions(IOptions<MvcJsonOptions> options, ArrayPool<char> arrayPool, LinkBuilderSettings settings)
        {
            _options = options.Value;
            _arrayPool = arrayPool;
            _settings = settings;
        }
        public void Configure(MvcOptions options)
        {
            if (_settings != null && _settings.AlwaysIncludeLinks)
            {
                options.OutputFormatters.RemoveType<JsonOutputFormatter>();
            }
            options.OutputFormatters.Add(new HyperlinkJsonOutputFormatter(_options.SerializerSettings, _arrayPool));
        }
    }
}
