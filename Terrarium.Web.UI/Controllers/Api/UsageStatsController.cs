using System.Net.Http;
using System.Web.Http;

namespace Terrarium.Web.UI.Controllers.Api
{
    public class UsageStatsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse();
        }
    }
}
