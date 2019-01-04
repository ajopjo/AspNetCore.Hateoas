using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AspNetCore.HypermediaLinks.Tests.Integration
{
    public class TestFixture<TStartUp> : IDisposable
    {
        private readonly TestServer _testServer;
        public HttpClient HttpClient { get; }

        public TestFixture(TestServerSettings settings)
        {
            var builder = new WebHostBuilder();
            builder.UseConfiguration(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            builder.UseStartup(typeof(TStartUp));

            HttpClient = _testServer.CreateClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(settings.MediaType));
        }
        public void Dispose()
        {
            _testServer.Dispose();
            HttpClient.Dispose();
        }
    }

    public class TestServerSettings
    {
        public string MediaType { get; set; }
    }
}
