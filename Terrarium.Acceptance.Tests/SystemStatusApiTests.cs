using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terrarium.Acceptance.Tests.Extensions;
using Terrarium.Web.UI;

namespace Terrarium.Acceptance.Tests
{
    [TestClass]
    public class SystemStatusApiTests
    {
        private const string RequestUri = "api/SystemStatus";

        [TestMethod]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = new HttpClientBuilder()
                                    .WithBuildStep(c => DependencyInjection.Setup(c))
                                    .Build()) {
                var response = client.GetAsync(RequestUri).Result;

                Assert.IsTrue(response.IsSuccessStatusCode, $"actual status code: {response.StatusCode}");
            }
        }

        [TestMethod]
        public void GetReturnsJsonContent()
        {
            using (var client = new HttpClientBuilder()
                                    .WithBuildStep(c => DependencyInjection.Setup(c))
                                    .Build()) {
                var response = client.GetAsync(RequestUri).Result;

                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                var json = response.Content.ReadAsJsonAsync().Result;
                Assert.IsNotNull(json);
            }
        }
    }
}
