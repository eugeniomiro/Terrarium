//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class PerformanceCounterCategoryInfo
    {
        public PerformanceCounterCategoryInfo()
        {
        }

        public PerformanceCounterCategoryInfo(string categoryName, string categoryHelp,
                                              CounterCreationDataInfo[] counterCreationDataInfos) : this()
        {
            CategoryHelp = categoryHelp;
            CategoryName = categoryName;
            CounterCreationDataInfos = counterCreationDataInfos;
        }

        [XmlAttribute("categoryHelp")]
        public string CategoryHelp { get; set; }

        [XmlAttribute("categoryName")]
        public string CategoryName { get; set; }

        public CounterCreationDataInfo[] CounterCreationDataInfos { get; set; }
    }
}