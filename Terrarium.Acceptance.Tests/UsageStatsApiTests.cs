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
            Uri baseAddress = new Uri("http://localhost:12345");

            using (var config = new HttpSelfHostConfiguration(baseAddress)) {
                WebApiConfig.Register(config);
                using (var server = new HttpSelfHostServer(config))
                using (var client = new HttpClient(server)) {
                    client.BaseAddress = baseAddress;
                    var response = client.GetAsync("api/Usage").Result;

                    Assert.IsTrue(response.IsSuccessStatusCode, $"actual status code: {response.StatusCode}");
                }
            }
        }
    }
}
