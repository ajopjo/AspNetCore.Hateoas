using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks
{
    public class HyperLinkModel
    {
        [JsonProperty(PropertyName = "_links")]
        public IList<Link> Links { get; set; }
    }


    public class Link
    {
        public string Rel { get; set; }
        public Uri Href { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
