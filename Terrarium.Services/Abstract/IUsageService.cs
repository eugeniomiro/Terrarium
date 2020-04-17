using Terrarium.Server.BugReporting;
using Terrarium.Server.UsageReporting;

namespace Terrarium.Server.Abstract
{
    interface IUsageService
    {
        void ReportUsage(UsageData data);
    }
}
