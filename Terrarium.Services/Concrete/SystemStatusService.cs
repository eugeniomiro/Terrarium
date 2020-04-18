using Terrarium.Domain;
using Terrarium.Services.Abstract;

namespace Terrarium.Services.Concrete
{
    public class SystemStatusService : ISystemStatusService
    {
        public SystemStatus GetSystemStatus()
        {
            return new SystemStatus();
        }
    }
}
