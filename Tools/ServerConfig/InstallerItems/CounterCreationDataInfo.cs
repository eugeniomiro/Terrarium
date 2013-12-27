//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Diagnostics;
using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class CounterCreationDataInfo
    {

        public CounterCreationDataInfo() { }

        public CounterCreationDataInfo(string counterName, string instanceName, PerformanceCounterType counterType,
                                       string counterHelp) : this()
        {
            this.CounterName = counterName;
            this.InstanceName = instanceName;
            this.CounterHelp = counterHelp;
            this.CounterType = counterType;
        }

        [XmlAttribute("counterHelp")]
        public string CounterHelp { get; set; }

        [XmlAttribute("counterName")]
        public string CounterName { get; set; }

        [XmlAttribute("instanceName")]
        public string InstanceName { get; set; }

        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlAttribute("counterType")]
        public PerformanceCounterType CounterType { get; set; }
    }
}