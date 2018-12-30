using AspNetCore.HypermediaLinks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Reflection.BindingFlags;

namespace AspNetCore.HypermediaLinks.Template
{
    public class TemplateLinkBuilder : ITemplateLinkBuilder
    {
        private readonly Uri _uri;
        private readonly StringBuilder _template;
        private readonly Link _link = new Link();

        public TemplateLinkBuilder(Uri uri, StringBuilder template)
        {
            Guard(uri, template);
            _uri = uri;
            _template = template;
        }

        public ITemplateLinkBuilder Type(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(method);
            }
            _link.Type = method.ToUpper();
            return this;
        }

        public ITemplateLinkBuilder Values(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "please add path values");
            }
            _template.ReplaceValues(obj);
            return this;
        }

        public ITemplateLinkBuilder Title(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException(title);
            _link.Title = title;
            return this;
        }

        public ITemplateLinkBuilder Name(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            _link.Name = name;
            return this;
        }

        public ITemplateLinkBuilder IsTemplate(bool isTemplate)
        {
            _link.Templated = isTemplate;
            return this;
        }
        public IRelBuilder Build()
        {
            _link.Href = new Uri(_uri, _template.ToString());
            return new RelBuilder(_link);
        }


        private void Guard(Uri uri, StringBuilder template)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri", "Uri is missing");
            }

            if (template.Length == 0)
            {
                throw new ArgumentNullException("template", "Add a path template");
            }
        }
    }
}
