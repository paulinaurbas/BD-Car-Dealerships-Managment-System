using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD2_CAR_DEAL.Controllers
{
    public class HomeController : Controller
    {
        private CarsEntities _db = new CarsEntities();
        public ActionResult Index()
        {
            return View(_db.Cars.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}