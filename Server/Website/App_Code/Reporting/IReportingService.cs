using System;
using Terrarium.Server;

interface IReportingService
{
    ReturnCode ReportPopulation(global::System.Data.DataSet data, Guid guid, int currentTick);
}
