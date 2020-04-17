using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Terrarium.Acceptance.Tests
{
    using Extensions;

    [TestClass]
    public class UsageStatsApiTests
    {
        private const string RequestUri = "api/UsageStats";

        [TestMethod]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create()) {
                var response = client.GetAsync(RequestUri).Result;

                Assert.IsTrue(response.IsSuccessStatusCode, $"actual status code: {response.StatusCode}");
            }
        }

        [TestMethod]
        public void GetReturnsJsonContent()
        {
            using (var client = HttpClientFactory.Create()) {
                var response = client.GetAsync(RequestUri).Result;

                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                var json = response.Content.ReadAsJsonAsync().Result;
                Assert.IsNotNull(json);
            }
        }
    }
}
