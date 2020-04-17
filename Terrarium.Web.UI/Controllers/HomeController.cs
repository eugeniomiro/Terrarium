using System.Collections.Generic;
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
            UserStatsViewModel vm = new UserStatsViewModel {
                UserAlias = "Eugenio",
                IndividualPeriod = new List<SelectListItem> {
                    new SelectListItem { Text="Today", Value = "Today", Selected=false },
                    new SelectListItem { Text="Week", Value = "Week", Selected=true } ,
                    new SelectListItem { Text="Total", Value = "Total", Selected=false }
                },
                Participants = new List<Participant> {
                    new Participant{ Alias="Alias1", Rank=20, Hours=345 },
                    new Participant{ Alias="Alias5", Rank=19, Hours=265 },
                    new Participant{ Alias="Alias1", Rank=18, Hours=165 },
                    new Participant{ Alias="Alias2", Rank=10, Hours=99 },
                    new Participant{ Alias="Alias6", Rank=5, Hours=4 },
                }
            };
            return View(vm);
        }

        public ActionResult Charts()
        {
            ChartsViewModel vm = new ChartsViewModel {
                Creatures   = new List<Creature>(),
                Filter      = new List<SelectListItem> {
                    new SelectListItem { Text = "Population", Selected = false, Value = "Population" },
                    new SelectListItem { Text = "Species Name", Selected = false, Value = "SpeciesName" },
                    new SelectListItem { Text = "Author Name", Selected = false, Value = "AuthorName" },
                    new SelectListItem { Text = "Type", Selected = false, Value = "Type" }
                },
                SelectedCreatures = new List<Creature>(),
                Versions = new List<SelectListItem>{
                    new SelectListItem { Text = "All Versions", Selected = true, Value = "" },
                    new SelectListItem { Text = "2.1.0", Selected = true, Value = "2.1.0" }
                },
                StartTimes = new List<SelectListItem> {
                    new SelectListItem { Text = "Last 24 Hours", Selected = true, Value = "" },
                }
            };

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult InfoBar()
        {
            InfoBarViewModel vm = new InfoBarViewModel {
                ServicesStatusText = "Up",
                ServicesStatusClass = "Up",
                DatabaseStatusText = "Up",
                DatabaseStatusClass = "Up",
                PeerCount = 0,
                Tip = "You can use Alt-Enter to enter a true Full-Screen view.",
                TopOrganisms = new Dictionary<OrganismType, TopOrganisms>()
            };
            vm.TopOrganisms[OrganismType.Plant] = new TopOrganisms() { 
                Species = new Species[] {
                    new Species{ Name= "Acelga",    Population=100 },
                    new Species{ Name= "Lechuga",   Population=99 },
                    new Species{ Name= "Tomate",    Population=98 },
                    new Species{ Name= "Banana",    Population=97 },
                    new Species{ Name= "Remolacha", Population=96 },
                }
            };
            vm.TopOrganisms[OrganismType.Herbivore] = new TopOrganisms {
                Species = new Species[] {
                    new Species{ Name= "Vaca",          Population=100 },
                    new Species{ Name= "Huemul",        Population=99 },
                    new Species{ Name= "Gliptodonte",   Population=98 },
                    new Species{ Name= "Herbivore",     Population=97 },
                    new Species{ Name= "Carlitos",      Population=96 },
                }
            };
            vm.TopOrganisms[OrganismType.Carnivore] = new TopOrganisms {
                Species = new Species[] {
                    new Species{ Name= "Cocodrilo", Population=100 },
                    new Species{ Name= "Tigre",     Population=99 },
                    new Species{ Name= "Mono",      Population=98 },
                    new Species{ Name= "Carnivore1", Population=97 },
                    new Species{ Name= "Juancito", Population=96 },
                }
            };
            
            return PartialView("_InfoBar", vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to .NET Terrarium 3.0";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Welcome to .NET Terrarium 3.0";

            return View();
        }
    }
}
