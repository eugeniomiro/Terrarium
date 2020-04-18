using System.Net;
using System.Net.Http;
using System.Web.Http;
using Terrarium.Services.Abstract;

namespace Terrarium.Web.UI.Controllers.Api
{

    public class SystemStatusController : ApiController
    {
        public SystemStatusController(ISystemStatusService systemStatusService)
        {
            _systemStatusService = systemStatusService;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _systemStatusService.GetSystemStatus());
        }

        private ISystemStatusService _systemStatusService;
    }
}
