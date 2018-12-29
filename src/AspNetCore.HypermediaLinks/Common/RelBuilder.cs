using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Common
{
    public class RelBuilder : IRelBuilder
    {
        private readonly Link _link;

        public RelBuilder(Link link)
        {
            _link = link;
        }

        public Link AddRel(string rel)
        {
            _link.Rel = rel?.ToLower();
            return _link;
        }

        public Link AddSelfRel()
        {
            return AddRel("self");
        }
    }
}
