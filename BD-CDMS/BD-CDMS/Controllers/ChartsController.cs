using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BD_CDMS.Models;
using Microsoft.AspNet.Identity;

namespace BD_CDMS.Controllers
{
    public class ChartsController : Controller
    {
        private Entities db = new Entities();

        // GET: Charts
        public ActionResult Index(string selectedChart)
        {
            ViewBag.selectedChart = selectedChart;
            return View();
        }


        public ActionResult SellsPerDate()
        {

            var order = db.Order.Select(n => new
            {
                id = n.Id,
                date = n.Date
            });

            ViewBag.data = order.ToList();

            var chart = new Chart(600, 400).AddSeries(name: "Price", yValues: order, yFields: "id", xValue: order, xField: "date").AddTitle("Cars sold by date");

            ViewBag.chart = chart;

            return View("Chart");
        }
    }
}