using AspNetCore.HypermediaLinks.Configuration;
using AspNetCore.HypermediaLinks.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("AspNetCore.HypermediaLinks.Tests")]
namespace AspNetCore.HypermediaLinks
{
    public class HypermediaBuilder : IHypermediaBuilder
    {
        private Uri _uri;
        private readonly IList<LinkConfiguration> _linkConfigurations;

        /// <summary>
        /// for moq testing
        /// </summary>
        internal HypermediaBuilder() { }

        internal HypermediaBuilder(IList<LinkConfiguration> linkConfigurations)
        {
            _linkConfigurations = linkConfigurations;
        }

        public HypermediaBuilder(Uri uri, IList<LinkConfiguration> linkConfigurations)
        {
            _uri = uri;
            _linkConfigurations = linkConfigurations;
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
            return Fromtemplate(new Uri(uri), template);
        }

        public IConfigurationLinkBuilder FormConfiguration(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            var linkConfig = _linkConfigurations.FirstOrDefault(l => l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (linkConfig == null)
                throw new ArgumentNullException("linkConfig", "Configuration setting is missing");

            if (linkConfig.Uri == null)
            {
                return new ConfigurationLinkBuilder(_uri, linkConfig);
            }
            return new ConfigurationLinkBuilder(linkConfig);
        }
    }

    public interface IHypermediaBuilder
    {
        ITemplateLinkBuilder Fromtemplate(string template);
        ITemplateLinkBuilder Fromtemplate(string uri, string template);

        ITemplateLinkBuilder Fromtemplate(Uri uri, string template);
    }
}
