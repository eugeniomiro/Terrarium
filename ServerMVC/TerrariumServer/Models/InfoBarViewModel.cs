using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrarium.Server;

namespace TerrariumServer.Models
{
    public class InfoBarViewModel
    {
        public String                                   ServicesStatusText { get; set; }
        public String                                   ServicesStatusClass { get; set; }
        public String                                   DatabaseStatusText { get; set; }
        public String                                   DatabaseStatusClass { get; set; }
        public Int32                                    PeerCount { get; set; }
        public Dictionary<OrganismType, TopOrganisms>   TopOrganisms { get; set; }
        public String                                   Tip { get; set; }
        public Exception                                Error { get; set; }
    }
}
