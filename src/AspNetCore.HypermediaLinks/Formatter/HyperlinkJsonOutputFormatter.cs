using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Buffers;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AspNetCore.HypermediaLinks.Formatter
{
    public class HyperlinkJsonOutputFormatter : JsonOutputFormatter
    {
        public HyperlinkJsonOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool) : base(serializerSettings, charPool)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/hal+json"));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var linkBuilder = serviceProvider.GetService(typeof(IHypermediaBuilder)) as HypermediaBuilder;

            if (context.Object is IEnumerable<HyperMediaSupportModel>)
            {
                foreach (var hypermediaModel in context.Object as IEnumerable<HyperMediaSupportModel>)
                {
                    hypermediaModel.AddHyperMediaSupportLinks(linkBuilder);
                }
            }

            if (context.Object is HyperMediaSupportModel model)
            {
                model.AddHyperMediaSupportLinks(linkBuilder);
            }

            return base.WriteResponseBodyAsync(context, selectedEncoding);
        }

    }
}
