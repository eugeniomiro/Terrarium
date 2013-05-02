using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TerrariumServer.Controllers
{
    public class DocumentationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult UserInterface()
        {
            return View();
        }

        public ActionResult CSTutorial(Int32? id)
        {
            if (id != null)
            {
                if (id < 1 || id > 4)
                    return View();
                return View("CSTutorial" + id.Value);
            }
            return View();
        }

        public ActionResult VBTutorial()
        {
            return View();
        }

        public ActionResult UsingSdk()
        {
            return View();
        }

        public ActionResult Samples(Int32? id, String kind, String lang)
        {
            if (id != null)
            {
                if (id >= 0 || id <= 3)
                {
                    if (kind == "carnivore" || kind == "herbivore")
                    {
                        if (lang == "cs" || lang == "vb")
                        {
                            return View("Samples_" + kind + lang + id);
                        }
                    }
                }
            }
            return View();
        }
    }
}
