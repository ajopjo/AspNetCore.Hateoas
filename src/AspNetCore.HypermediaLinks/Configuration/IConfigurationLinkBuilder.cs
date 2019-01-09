using AspNetCore.HypermediaLinks.Common;

namespace AspNetCore.HypermediaLinks.Configuration
{
    public interface IConfigurationLinkBuilder
    {
        IRelBuilder Build();
    }
}
