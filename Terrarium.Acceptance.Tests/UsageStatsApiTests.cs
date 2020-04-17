using System;
using System.Net.Http;
using System.Web.Http.SelfHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TerrariumServer;

namespace Terrarium.Acceptance.Tests
{
    [TestClass]
    public class UsageStatsApiTests
    {
        [TestMethod]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = CreateHttpClient()) {
                var response = client.GetAsync("api/Usage").Result;

                Assert.IsTrue(response.IsSuccessStatusCode, $"actual status code: {response.StatusCode}");
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var baseAddress = new Uri("http://localhost:12345");
            var config      = new HttpSelfHostConfiguration(baseAddress);

            WebApiConfig.Register(config);
            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server);
            try {
                client.BaseAddress = baseAddress ;
                return client; 
            } catch {
                server.Dispose();
                client.Dispose();
                throw;
            } finally {
                config.Dispose();
            }
        }
    }
}
