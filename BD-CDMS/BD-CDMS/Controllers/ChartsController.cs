using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BD_CDMS.Models;
using Microsoft.AspNet.Identity;
using Rotativa;

namespace BD_CDMS.Controllers
{
    public class ChartsController : Controller
    {
        private Entities db = new Entities();

        // GET: Charts
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Index(string selectedChart)
        {
            ViewBag.selectedChart = selectedChart;
            return View();
        }


        public ActionResult SellsPerDate()
        {

            var order = db.Order.Select(n => new
            {
                x = n.Date,
                y = n.Id
            });

            ViewBag.data = order.ToList();

            var chart = new Chart(600, 400).AddSeries(name: "Price", yValues: order, yFields: "y", xValue: order, xField: "x").AddTitle("Cars sold by date");

            ViewBag.chart = chart;

            return View("Chart");
        }

        public ActionResult IncomeForWeek()
        {

            var order = db.Order.Select(n => new
            {
                x = n.Date,
                y = n.Car.Price
            });

            var order2 = order.GroupBy(i => i.x).Select(n => new
            {
                x = n.Key,
                y = n.Sum(a=>a.y)
            });

            //ViewBag.data = order2.ToList();

            var chart = new Chart(600, 400).AddSeries(name: "Price", yValues: order2, yFields: "y", xValue: order2, xField: "x").AddTitle("Income by date");

            ViewBag.chart = chart;

            return View("Chart");
        }

        public ActionResult SoldBrand()
        {

            var order = db.Order.Select(n => new
            {
                x = n.Car.Brand,
                y = n.Id
            });

            var order2 = order.GroupBy(i => i.x).Select(n => new
            {
                x = n.Key,
                y = n.Sum(a => a.y)
            });

            //ViewBag.data = order2.ToList();

            var chart = new Chart(600, 400).AddSeries(name: "Price", yValues: order2, yFields: "y", xValue: order2, xField: "x",chartType:"pie").AddTitle("Cars sold by brand");

            ViewBag.chart = chart;

            return View("Chart");
        }

        public ActionResult Workers()
        {

            var order = db.AspNetRoles.Select(n => new
            {
                x = n.Name,
                y = n.Id
            });

            var order2 = order.GroupBy(i => i.x).Select(n => new
            {
                x = n.Key,
                y = n.Count()
            });

            //ViewBag.data = order2.ToList();

            var chart = new Chart(600, 400).AddSeries(name: "Price", yValues: order2, yFields: "y", xValue: order2, xField: "x", chartType: "pie").AddTitle("Workers");

            ViewBag.chart = chart;

            return View("Chart");
        }


        public ActionResult CreatePDF()
        {
            var report = new ViewAsPdf("CreatePDF");

            return report;
        }

    }
}