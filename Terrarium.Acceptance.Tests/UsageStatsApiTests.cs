using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Terrarium.Acceptance.Tests
{
    [TestClass]
    public class UsageStatsApiTests
    {
        [TestMethod]
        public void GetReturnsResponseWithCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create()) {
                var response = client.GetAsync("api/Usage").Result;

                Assert.IsTrue(response.IsSuccessStatusCode, $"actual status code: {response.StatusCode}");
            }
        }
    }
}
