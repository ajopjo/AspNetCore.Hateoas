using AspNetCore.HypermediaLinks.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.HypermediaLinks.Template
{
    public interface ITemplateLinkBuilder
    {
        //ITemplateLinkBuilder Values(object obj);
        ITemplateLinkBuilder Type(string method);
        ITemplateLinkBuilder Title(string title);
        ITemplateLinkBuilder Name(string name);
        ITemplateLinkBuilder IsTemplate(bool isTemplate);
        IRelBuilder Build();
    }
}
