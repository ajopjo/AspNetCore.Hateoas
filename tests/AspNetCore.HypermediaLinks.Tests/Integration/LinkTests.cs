using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class LinkTests
    {
        [Fact]
        public async void LinkTestWithSimpleResponseModel()
        {
            using (var host = new TestFixture<FakeStartup>(new TestServerSettings() { MediaType = $"application/hal+json" }))
            {
                var response = await host.HttpClient.GetAsync("api/fake/fakeModel?id=20AAA6E6-8562-4CC7-8567-09E55AB4C6D6&name=Ajo");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);

            }
        }
    }
}
