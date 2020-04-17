using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Terrarium.Acceptance.Tests
{
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
    }
}
