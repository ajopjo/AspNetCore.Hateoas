using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var response = await host.HttpClient.GetAsync("api/fake/fakeModel?id=1&name=Ajo");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);

                Assert.Equal(1, model.Id);
                Assert.Equal("https://localhost/api/fake/fakeModel?id=1&name=Ajo", model.Links.FirstOrDefault().Href.ToString());
            }
        }
    }
}
