using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TerrariumServer.Models
{
    public class ChartsViewModel
    {
        public  IEnumerable<Creature>       Creatures           { get; set; }
        public  IEnumerable<SelectListItem> Versions            { get; set; }
        public  IEnumerable<SelectListItem> Filter              { get; set; }
        public  IEnumerable<SelectListItem> StartTimes          { get; set; }
        public  IEnumerable<Creature>       SelectedCreatures   { get; set; }
        
    }
}