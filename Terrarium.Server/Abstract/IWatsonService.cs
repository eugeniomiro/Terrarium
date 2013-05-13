using System.Data;

namespace Terrarium.Server.Abstract
{
    interface IWatsonService
    {
        void ReportError(DataSet data);
    }
}
