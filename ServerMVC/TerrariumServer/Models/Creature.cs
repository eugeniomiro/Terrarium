using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariumServer.Models
{
    public class Creature
    {
        public  String  Charting    { get; set; }
        public  int     Population  { get; set; }
        public  String  SpeciesName { get; set; }
        public  String  AuthorName  { get; set; }
        public  String  Type        { get; set; }

        public object BirthCount { get; set; }

        public object StarvedCount { get; set; }

        public object KilledCount { get; set; }

        public object ErrorCount { get; set; }

        public object TimeoutCount { get; set; }

        public object SickCount { get; set; }

        public object OldAgeCount { get; set; }

        public object SecurityViolation { get; set; }
    }
}
