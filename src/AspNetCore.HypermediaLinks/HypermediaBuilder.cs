using AspNetCore.HypermediaLinks.Configuration;
using AspNetCore.HypermediaLinks.Template;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("AspNetCore.HypermediaLinks.Tests")]
namespace AspNetCore.HypermediaLinks
{
    public class HypermediaBuilder : IHypermediaBuilder
    {
        private Uri _uri;
        private readonly IList<LinkConfiguration> _linkConfigurations;
        private readonly IUrlHelper _urlHelper;

        #region Ctr
        internal HypermediaBuilder() { }
        internal HypermediaBuilder(IList<LinkConfiguration> linkConfigurations)
        {
            _linkConfigurations = linkConfigurations;
        }
        public HypermediaBuilder(Uri uri, IList<LinkConfiguration> linkConfigurations, IUrlHelper urlHelper)
        {
            _uri = uri;
            _linkConfigurations = linkConfigurations;
            _urlHelper = urlHelper;
        }
        #endregion

        public ITemplateLinkBuilder Fromtemplate(string template, object values = null, string uri = null)
        {
            if (string.IsNullOrEmpty(uri))
                return Fromtemplate(template, values, _uri);

            return Fromtemplate(template, values, new Uri(uri));
        }
        public ITemplateLinkBuilder Fromtemplate(string template, object values = null, Uri uri = null)
        {
            return new TemplateLinkBuilder(uri ?? _uri, new StringBuilder(template), values);
        }
        public IConfigurationLinkBuilder FormConfiguration(string name, object values = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(name);
            var linkConfig = _linkConfigurations.FirstOrDefault(l => l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (linkConfig == null)
                throw new ArgumentNullException("linkConfig", "Configuration setting is missing");

            if (linkConfig.Uri == null)
            {
                linkConfig.Uri = _uri;
            }
            return new ConfigurationLinkBuilder(linkConfig, values);
        }
        public ITemplateLinkBuilder FromController<T>(Expression<Func<T, Func<IActionResult>>> action, object values = null) where T : ControllerBase
        {
            Type t = typeof(T);
            var controllerName = t.GetControllerName<T>();
            var actionName = action.GetActionName();
            var link = _urlHelper.Action(actionName, controllerName, values, _uri.Scheme, _uri.Host);
            return new TemplateLinkBuilder(link);
        }
        public ITemplateLinkBuilder FromController<T>(Expression<Func<T, string>> action, object values = null) where T : ControllerBase
        {
            if (!(action.Body is ConstantExpression constExp))
                throw new ArgumentException("Action name must be a constant expression");
            Type t = typeof(T);
            var controllerName = t.GetControllerName<T>();
            var actionName = constExp.Value?.ToString();
            var link = _urlHelper.Action(actionName, controllerName, values, _uri.Scheme, _uri.Host);
            return new TemplateLinkBuilder(link);
        }
    }

    public interface IHypermediaBuilder
    {
        ITemplateLinkBuilder Fromtemplate(string template, object values = null, string uri = null);
        ITemplateLinkBuilder Fromtemplate(string template, object values = null, Uri uri = null);
        ITemplateLinkBuilder FromController<T>(Expression<Func<T, Func<IActionResult>>> action, object values = null) where T : ControllerBase;
        ITemplateLinkBuilder FromController<T>(Expression<Func<T, string>> action, object values = null) where T : ControllerBase;
        IConfigurationLinkBuilder FormConfiguration(string name, object values = null);
    }
}
