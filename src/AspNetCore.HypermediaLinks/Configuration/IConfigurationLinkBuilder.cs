using AspNetCore.HypermediaLinks.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Configuration
{
    public interface IConfigurationLinkBuilder
    {
        Link Values(object obj);

        Link Build();
    }
}
