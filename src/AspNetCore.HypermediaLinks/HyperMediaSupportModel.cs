using Newtonsoft.Json;
using System;
using System.Collections;
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
        private List<KeyValuePair<string, Link>> _links = new List<KeyValuePair<string, Link>>();

        [JsonProperty(PropertyName = "_links")]
        public IEnumerable<KeyValuePair<string, Link>> Links { get => _links; }

        protected internal void Add(Link link)
        {
            var kvp = new KeyValuePair<string, Link>(link.Rel, link);

            if (_links.Contains(kvp, new KeyComparer()))
            {
                throw new ArgumentException("Link rel already exists", link.Rel);
            }
            _links.Add(kvp);
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
    }

    /// <summary>
    /// Model for Hypermedia Links
    /// </summary>
    public class Link
    {
        public string Rel { get; set; }
        public Uri Href { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
        internal void Add(KeyValuePair<string, string> kvp)
        {
            if (Attributes.ContainsKey(kvp.Key))
            {
                Attributes[kvp.Key] = kvp.Value;
            }
            else
            {
                Attributes.Add(kvp.Key, kvp.Value);
            }
        }
    }

    public class KeyComparer : IEqualityComparer<KeyValuePair<string, Link>>
    {
        public bool Equals(KeyValuePair<string, Link> x, KeyValuePair<string, Link> y)
        {
            return x.Key.Equals(y.Key, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(KeyValuePair<string, Link> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
