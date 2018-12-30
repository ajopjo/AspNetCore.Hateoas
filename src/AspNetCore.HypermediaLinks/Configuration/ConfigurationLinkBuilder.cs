using System;
using System.Text;
using AspNetCore.HypermediaLinks.Common;

namespace AspNetCore.HypermediaLinks.Configuration
{
    public class ConfigurationLinkBuilder : IConfigurationLinkBuilder
    {
        private readonly Link _link;
        private readonly Uri _uri;
        private readonly StringBuilder _template;

        public ConfigurationLinkBuilder(LinkConfiguration linkConfiguration) : this(linkConfiguration.Uri, linkConfiguration)
        {
        }

        public ConfigurationLinkBuilder(Uri uri, LinkConfiguration linkConfiguration)
        {
            _uri = uri;
            Guard(linkConfiguration);
            _template = new StringBuilder(linkConfiguration.Path);
            _link = new Link()
            {
                Type = linkConfiguration.Type.ToUpper(),
                Rel = linkConfiguration.Rel,
                Templated = linkConfiguration.Templated,
                Title = linkConfiguration.Title,
                Name = linkConfiguration.Name
            };
        }

        public Link Build()
        {
            _link.Href = new Uri(_uri, _template.ToString());
            return _link;
        }

        public Link Values(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj", "Values for uri path is null");
            _template.ReplaceValues(obj);
            _link.Href = new Uri(_uri, _template.ToString());

            return _link;
        }

       
        private void Guard(LinkConfiguration linkConfiguration)
        {
            if (string.IsNullOrEmpty(linkConfiguration.Path))
            {
                throw new ArgumentNullException(linkConfiguration.Path, "path template is missing");
            }

            if (string.IsNullOrEmpty(linkConfiguration.Type))
            {
                throw new ArgumentNullException(linkConfiguration.Type, "Method|Action is missing");
            }

        }
    }
}
