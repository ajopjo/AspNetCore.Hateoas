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
            using (var host = new TestFixture<FakeStartup>(new TestServerSettings() { MediaType = $"application/json" }))
            {
                var response = await host.HttpClient.GetAsync("api/fake/simple");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);


            }
        }
    }
}
