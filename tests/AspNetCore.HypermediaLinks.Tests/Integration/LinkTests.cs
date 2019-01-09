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
                var response = await host.HttpClient.GetAsync("api/fake/fakeModel?id=1&name=test");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);

                Assert.Equal(1, model.Id);
                Assert.Equal("http://localhost/api/fake/fakeModel?id=1&name=test", model.Links.FirstOrDefault().Href.ToString());
            }
        }

        [Fact]
        public async void LinkTestWithSimpleResponseModelNoHalHeader()
        {
            using (var host = new TestFixture<FakeStartup>())
            {
                var response = await host.HttpClient.GetAsync("api/fake/fakeModel?id=1&name=test");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);

                Assert.Equal(1, model.Id);
                Assert.Equal(0, model.Links.Count());
            }
        }

        [Fact]
        public async void LinkTestWithSimpleResponseModelRenderLinks()
        {
            using (var host = new TestFixture<FakeAlwaysLinkStartUp>())
            {
                var response = await host.HttpClient.GetAsync("api/fake/fakeModel?id=1&name=test");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeModel>(jsonRes);

                Assert.Equal(1, model.Id);
                Assert.Equal("http://localhost/api/fake/fakeModel?id=1&name=test", model.Links.FirstOrDefault().Href.ToString());
            }
        }

        [Fact]
        public async void LinkTestWithCollectionsResponseModel()
        {
            using (var host = new TestFixture<FakeStartup>(new TestServerSettings() { MediaType = $"application/hal+json" }))
            {
                var response = await host.HttpClient.GetAsync("api/fake/fakeArrayModel?id=1&name=test");
                response.EnsureSuccessStatusCode();

                var jsonRes = await response.Content.ReadAsStringAsync();

                var model = JsonConvert.DeserializeObject<FakeArrayModel>(jsonRes);

                Assert.Equal(1, model.Id);
                Assert.Equal(1, model.Links.Count());
                Assert.Equal("http://localhost/api/fake/fakeArrayModel?id=1&name=test", model.Links.FirstOrDefault().Href.ToString());

                var arrModelHref = model.FakeModelsArrays[0].Links.FirstOrDefault().Href;
                Assert.Equal("http://localhost/api/fake/fakeModel?id=1&name=test1", arrModelHref.ToString());

                var listModelHref = model.FakeModelList[0].Links.FirstOrDefault().Href;
                Assert.Equal("http://localhost/api/fake/fakeModel?id=3&name=test3", listModelHref.ToString());

                var ienumModelHref = model.FakeModels.FirstOrDefault().Links.FirstOrDefault().Href;
                Assert.Equal("http://localhost/api/fake/fakeModel?id=5&name=test5", ienumModelHref.ToString());

            }
        }


    }
}
