//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System.Web.Services;
namespace Terrarium.Server
{
    /// <summary>
    /// Summary description for BugService
    /// </summary>


    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BugService : WebService, IBugService
    {
        [WebMethod]
        public void ReportBug(Bug bug)
        {
            //TODO: Add code to report bugs
        }
    }

    public class Bug
    {
        public string Title;
        public string Description;
        public string Path;
        public string Alias;
        public string Version;
    }
}
