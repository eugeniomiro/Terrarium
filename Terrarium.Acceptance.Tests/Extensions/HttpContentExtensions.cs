using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Terrarium.Acceptance.Tests.Extensions
{
    internal static class HttpContentExtensions
    {
        internal static Task<object> ReadAsJsonAsync(this HttpContent response)
        {
            return response.ReadAsStringAsync().ContinueWith(t => JsonConvert.DeserializeObject(t.Result));
        }
    }
}
