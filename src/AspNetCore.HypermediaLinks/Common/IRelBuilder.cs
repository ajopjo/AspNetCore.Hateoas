using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Common
{
    public interface IRelBuilder
    {
        Link AddSelfRel();
        Link AddRel(string rel);
    }

    
}
