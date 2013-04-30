using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Terrarium.Server;
using TerrariumServer.Models;

namespace TerrariumServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to .NET Terrarium 3.0";

            return View();
        }

        public ActionResult UsageStats()
        {
            UserStatsViewModel vm   = new UserStatsViewModel();
            vm.UserAlias            = "Eugenio";
            vm.IndividualPeriod     = new List<SelectListItem>();
            vm.Participants         = new List<Participant>();
            return View(vm);
        }

        public ActionResult Documentation()
        {
            return View();
        }

        public ActionResult Charts()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult InfoBar()
        {
            InfoBarViewModel vm = new InfoBarViewModel();

            vm.ServicesStatusText = "Up";
            vm.ServicesStatusClass = "Up";
            vm.DatabaseStatusText = "Up";
            vm.DatabaseStatusClass = "Up";
            vm.PeerCount = 0;
            vm.Tip = "You can use Alt-Enter to enter a true Full-Screen view.";
            vm.TopOrganisms = new Dictionary<OrganismType, TopOrganisms>();
            vm.TopOrganisms[OrganismType.Plant] = new TopOrganisms();
            vm.TopOrganisms[OrganismType.Herbivore] = new TopOrganisms();
            vm.TopOrganisms[OrganismType.Carnivore] = new TopOrganisms();
            vm.TopOrganisms[OrganismType.Carnivore].Species = new Species[] { 
                new Species{ Name= "Cocodrilo", Population=100 },
                new Species{ Name= "Tigre",     Population=99 },
                new Species{ Name= "Mono", Population=98 },
                new Species{ Name= "Carnivore1", Population=97 },
                new Species{ Name= "Juancito", Population=96 },
            };
            vm.TopOrganisms[OrganismType.Plant].Species = new Species[] { 
                new Species{ Name= "Acelga", Population=100 },
                new Species{ Name= "Lechuga",     Population=99 },
                new Species{ Name= "Tomate", Population=98 },
                new Species{ Name= "Banana", Population=97 },
                new Species{ Name= "Remolacha", Population=96 },
            };
            vm.TopOrganisms[OrganismType.Herbivore].Species = new Species[] { 
                new Species{ Name= "Vaca", Population=100 },
                new Species{ Name= "Huemul",     Population=99 },
                new Species{ Name= "Gliptodonte", Population=98 },
                new Species{ Name= "Herbivore", Population=97 },
                new Species{ Name= "Carlitos", Population=96 },
            };  
            return PartialView("_InfoBar", vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
