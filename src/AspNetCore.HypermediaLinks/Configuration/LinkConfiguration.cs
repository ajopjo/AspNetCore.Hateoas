using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Configuration
{
    public class LinkConfiguration
    {
        public string Rel { get; set; }
        public Uri Uri { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }

        //optional
        public bool Templated { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
    }
}
