using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Template
{
    public interface ITemplateBuilder
    {
        ITemplateBuilder Fromtemplate(string template);

        ITemplateBuilder Fromtemplate(Uri uri, string template);
    }
}
