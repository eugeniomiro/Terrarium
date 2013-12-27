//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------------------------

using System.Xml.Serialization;

namespace Terrarium.Server
{
    public class EventLogInfo
    {
        public EventLogInfo()
        {
            ID = "";
            Source = "";
        }

        public EventLogInfo(string source) 
            : this()
        {
            source = source;
        }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("id")]
        public string ID { get; set; }
    }
}