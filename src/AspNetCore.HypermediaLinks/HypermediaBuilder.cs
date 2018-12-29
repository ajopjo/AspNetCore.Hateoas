using AspNetCore.HypermediaLinks.Template;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("AspNetCore.Hateoas.Tests")]
namespace AspNetCore.HypermediaLinks
{
    public class HypermediaBuilder : IHypermediaBuilder
    {
        private Uri _uri;

        /// <summary>
        /// for moq testing
        /// </summary>
        internal HypermediaBuilder()
        {

        }

        public HypermediaBuilder(Uri uri)
        {
            _uri = uri;
        }

        public ITemplateLinkBuilder Fromtemplate(string template)
        {
            return Fromtemplate(_uri, template);
        }

        public ITemplateLinkBuilder Fromtemplate(Uri uri, string template)
        {
            return new TemplateLinkBuilder(uri, new StringBuilder(template));
        }

        public ITemplateLinkBuilder Fromtemplate(string uri, string template)
        {
            return new TemplateLinkBuilder(new Uri(uri), new StringBuilder(template));
        }
    }

    public interface IHypermediaBuilder
    {
        ITemplateLinkBuilder Fromtemplate(string template);
        ITemplateLinkBuilder Fromtemplate(string uri, string template);

        ITemplateLinkBuilder Fromtemplate(Uri uri, string template);
    }
}
