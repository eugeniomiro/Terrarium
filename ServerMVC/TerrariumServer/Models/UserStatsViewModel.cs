using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TerrariumServer.Models
{
    public class UserStatsViewModel
    {
        public String                       UserAlias   { get; set; }
        public Int32                        TodaysHours { get; set; }
        public Int32                        WeekHours   { get; set; }
        public Int32                        TotalHours  { get; set; }
        public IEnumerable<SelectListItem>  IndividualPeriod { get; set; }
        public IEnumerable<Participant>     Participants { get; set; }
    }
}