using AspNetCore.HypermediaLinks.Configuration;
using AspNetCore.HypermediaLinks.Template;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        public HypermediaBuilder(IUrlHelper urlHelper, IOptions<List<LinkConfiguration>> linkConfigurations = null)
        {
            _linkConfigurations = linkConfigurations?.Value;
            _urlHelper = urlHelper;
            var builder = new UriBuilder(_urlHelper.ActionContext.HttpContext.Request.Host.Value)
            {
                Scheme = _urlHelper.ActionContext.HttpContext.Request.Scheme
            };
            _uri = builder.Uri;
        }
        #endregion

        /// <summary>
        /// Create a link from a string template
        /// </summary>
        /// <param name="template">string url template</param>
        /// <param name="values">values to be added on the url</param>
        /// <param name="uri">url host and schema, if its empty, the host and schema of the incoming request is added as uri</param>
        /// <returns></returns>
        public ITemplateLinkBuilder Fromtemplate(string template, object values = null, string uri = null)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return Fromtemplate(template, values, _uri);
            }

            return Fromtemplate(template, values, new Uri(uri));
        }
        /// <summary>
        /// Generate links from string template
        /// </summary>
        /// <param name="template">string template</param>
        /// <param name="values">if template contains values pass values in an optional anonymous object</param>
        /// <param name="uri">scheme+hostname, if value is null the incoming request's host name and scheme is taken</param>
        /// <returns></returns>
        public ITemplateLinkBuilder Fromtemplate(string template, object values = null, Uri uri = null)
        {
            return new TemplateLinkBuilder(uri ?? _uri, new StringBuilder(template), values);
        }

        /// <summary>
        /// generate link from configuration file
        /// </summary>
        /// <param name="name">name of the config settings</param>
        /// <param name="values">optional input values passed as anonymous object</param>
        /// <returns></returns>
        public IConfigurationLinkBuilder FormConfiguration(string name, object values = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            var linkConfig = _linkConfigurations.FirstOrDefault(l => l.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (linkConfig == null)
            {
                throw new ArgumentNullException("linkConfig", "Configuration setting is missing");
            }

            if (linkConfig.Uri == null)
            {
                linkConfig.Uri = _uri;
            }
            return new ConfigurationLinkBuilder(linkConfig, values);
        }

        /// <summary>
        /// generate link from the controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">name of action method</param>
        /// <param name="values">optional input values - anonymous object</param>
        /// <returns></returns>
        public ITemplateLinkBuilder FromController<T>(Expression<Func<T, string>> action, object values = null) where T : ControllerBase
        {
            if (!(action.Body is ConstantExpression constExp))
            {
                throw new ArgumentException("Action name must be a constant expression");
            }

            Type t = typeof(T);
            var controllerName = t.GetControllerName<T>();
            var actionName = constExp.Value?.ToString();
            var link = _urlHelper.Action(actionName, controllerName, values, _uri.Scheme, _uri.Host);
            return new TemplateLinkBuilder(link);
        }

        //This is not used as the expression takes a lot of time 

        //public ITemplateLinkBuilder FromController<T>(Expression<Func<T, object>> action) where T : ControllerBase
        //{
        //    Type t = typeof(T);
        //    var link = _urlHelper.Action(new UrlActionContext()
        //    {
        //        Controller = t.GetControllerName<T>(),
        //        Action = action.GetActionName(),
        //        Values = action.GetRouteValues(),
        //        Host = _uri.Host,
        //        Protocol = _uri.Scheme
        //    });
        //    return new TemplateLinkBuilder(link);
        //}
    }

    public interface IHypermediaBuilder
    {
        ITemplateLinkBuilder Fromtemplate(string template, object values = null, string uri = null);
        ITemplateLinkBuilder Fromtemplate(string template, object values = null, Uri uri = null);
        ITemplateLinkBuilder FromController<T>(Expression<Func<T, string>> action, object values = null) where T : ControllerBase;
        //ITemplateLinkBuilder FromController<T>(Expression<Func<T, object>> action) where T : ControllerBase;
        IConfigurationLinkBuilder FormConfiguration(string name, object values = null);
    }
}
