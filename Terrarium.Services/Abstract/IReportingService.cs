using System;
using System.Data;
using Terrarium.Server.ReportPopulation;

namespace Terrarium.Server.Abstract
{
    interface IReportingService
    {
        ReturnCode ReportPopulation(global::System.Data.DataSet data, Guid guid, int currentTick);
    }
}
