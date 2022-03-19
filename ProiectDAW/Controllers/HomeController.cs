using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Descrierea aplicației";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contactează-ne";

            return View();
        }

        public ActionResult Personal(PersoanaU p)
        {
            ViewBag.Message = "Pagina personală.";
            if (p.id == null)
            {
                p.id = -1;
            }
            ViewBag.id = p.id;

            return View();
        }

        [HttpPost]
        [ActionName("Personal")]
        public ActionResult Personal_Post()
        {
            ViewBag.Message = "Pagina personală.";

            return View();
        }
    }
}