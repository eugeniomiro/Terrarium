using System;
using Terrarium.Server;

interface IUsageService
{
    void ReportUsage(UsageData data);
}
