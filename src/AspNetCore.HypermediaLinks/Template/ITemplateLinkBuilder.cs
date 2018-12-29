using AspNetCore.HypermediaLinks.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Template
{
    public interface ITemplateLinkBuilder
    {
        ITemplateLinkBuilder Values(object obj);
        ITemplateLinkBuilder Type(string method);
        ITemplateLinkBuilder Attribute(string key, string value);
        IRelBuilder Then();
    }
}
