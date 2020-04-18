using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Terrarium.Web.UI.Controllers.Api
{
    using Domain;

    public class UsageStatsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new UsageData());
        }
    }
}
