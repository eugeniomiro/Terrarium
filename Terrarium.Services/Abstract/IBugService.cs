using Terrarium.Server.BugReporting;

namespace Terrarium.Server.Abstract
{
    interface IBugService
    {
        void ReportBug(Bug bug);
    }
}
