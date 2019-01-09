using AspNetCore.HypermediaLinks.Common;
using System;
using System.Text;

namespace AspNetCore.HypermediaLinks.Configuration
{
    public class ConfigurationLinkBuilder : IConfigurationLinkBuilder
    {
        private readonly Link _link;       


        public ConfigurationLinkBuilder(LinkConfiguration linkConfiguration, object values = null)
        {
            Guard(linkConfiguration);
             var template = new StringBuilder(linkConfiguration.Path);
            if (values != null)
                template.ReplaceValues(values);
            _link = new Link()
            {
                Type = linkConfiguration.Type?.ToUpper(),               
                Templated = linkConfiguration.Templated,
                Title = linkConfiguration.Title,
                Name = linkConfiguration.Name,
                Href = new Uri(linkConfiguration.Uri, template.ToString())
            };
        }

        public IRelBuilder Build()
        {

            return new RelBuilder(_link);
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
