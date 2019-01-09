using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using static System.Reflection.BindingFlags;
[assembly: InternalsVisibleTo("AspNetCore.HypermediaLinks.Tests")]
namespace AspNetCore.HypermediaLinks
{
    public abstract class HyperMediaSupportModel
    {

        /// <summary>
        /// Support for HAL or Hateoas
        /// </summary>  
        private List<Link> _links = new List<Link>();

        [JsonProperty(PropertyName = "_links")]
        public IEnumerable<Link> Links { get => _links; }

        protected internal void Add(Link link)
        {

            if (_links.Contains(link, new KeyComparer()))
            {
                throw new ArgumentException("Link rel already exists", link.Rel);
            }
            _links.Add(link);
        }

        internal void AddHyperMediaSupportLinks(HypermediaBuilder builder)
        {
            var props = GetType().GetProperties(Instance | Public | DeclaredOnly);

            if (props.Any())
            {
                foreach (var prop in props)
                {
                    if (prop.IsLinkSupportModel())
                    {
                        AddLinksToModel(prop, builder);
                    }
                    else if (prop.IsLinkSupportArray() || prop.IsLinkSupportGenericType())
                    {
                        AddLinksToArrayModel(prop, builder);
                    }
                }
            }
            AddHypermediaLinks(builder);
        }

        /// <summary>
        /// This method MUST be implemented by the child class and
        /// contains links|relation ship that resource holds with other
        /// resources
        /// </summary>
        /// <param name="builder"></param>
        public abstract void AddHypermediaLinks(HypermediaBuilder builder);

        private void AddLinksToModel(PropertyInfo prop, HypermediaBuilder builder)
        {
            var model = prop.GetValue(this) as HyperMediaSupportModel;
            model?.AddHyperMediaSupportLinks(builder);
        }

        private void AddLinksToArrayModel(PropertyInfo prop, HypermediaBuilder builder)
        {
            var models = prop.GetValue(this) as IEnumerable<HyperMediaSupportModel>;
            if (models.Any())
            {
                foreach (var model in models)
                {
                    model.AddHyperMediaSupportLinks(builder);
                }
            }
        }

        public bool ShouldSerializeLinks()
        {
            return (_links.Count > 0);
        }
    }

    /// <summary>
    /// Model for Hypermedia Links
    /// </summary>
    public class Link
    {
        public string Rel { get; set; }
        public Uri Href { get; set; }
        public string Type { get; set; }
        public bool? Templated { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }

    public class KeyComparer : IEqualityComparer<Link>
    {
        public bool Equals(Link x, Link y)
        {
            return x.Rel.Equals(y.Rel, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Link obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
