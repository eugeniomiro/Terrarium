using System.Net.Http;
using System.Web.Http;

namespace Terrarium.Web.UI.Controllers.Api
{
    public class UsageController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse();
        }
    }
}
