using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD_CDMS.Models;

namespace BD_CDMS.Controllers
{
    public class FeaturesController : Controller
    {
        private Entities db = new Entities();

        [Authorize(Roles = "Admin,Seller,Serviceman,Manager")]
        // GET: Features
        public ActionResult Index()
        {

            var feature = db.Feature.Include(f => f.Car);
            return View(feature.ToList());
        }

        [Authorize(Roles = "Admin,Seller,Serviceman,Manager")]
        // GET: Features/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Feature.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        [Authorize(Roles = "Admin,Serviceman")]
        // GET: Features/Create
        public ActionResult Create()
        {
            var cars = db.Car.Select(n => new
            {
                Id = n.Id,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).ToList();

            ViewBag.IdCar = new SelectList(cars, "Id", "Description");
            //ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand");

            return View();
        }

        // POST: Features/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Name,Description,IdCar")] Feature feature)
        {
            if (db.Feature.Select(n => n).Where(c => c.IdCar == feature.IdCar).Where(d => d.Type == feature.Type).Count() == 0) 
            {
                if (ModelState.IsValid)
                {
                    db.Feature.Add(feature);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", feature.IdCar);
            return View(feature);
        }

        [Authorize(Roles = "Admin,Serviceman")]
        // GET: Features/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Feature.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }

            var cars = db.Car.Select(n => new
            {
                Id = n.Id,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).ToList();

            ViewBag.IdCar = new SelectList(cars, "Id", "Description");
            //ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", feature.IdCar);
            return View(feature);
        }

        // POST: Features/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Name,Description,IdCar")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", feature.IdCar);
            return View(feature);
        }

        [Authorize(Roles = "Admin,Serviceman")]
        // GET: Features/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Feature.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }

        // POST: Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feature feature = db.Feature.Find(id);
            db.Feature.Remove(feature);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin,Seller,Serviceman,Manager")]
        public ActionResult IndexCar(int? id)
        {

            ViewBag.IdCar = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var feature = db.Feature.Select(n => n).Where(c => c.IdCar == id);

            //var feature = db.Feature.Include(f => f.Car);
            return View(feature.ToList());
        }

        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult CreateCar(int? id)
        {
            //ViewBag.IdCar = new SelectList(db.Car, "Id", "VIN");
            //var car = _db.Car.Select(n => n).Where(c => c.IdSold == false);
            ViewBag.IdCar = new SelectList(db.Car.Select(n => n).Where(c => c.Id == id), "Id", "VIN");
            return View();
        }

        // POST: Features/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCar([Bind(Include = "Id,Type,Name,Description,IdCar")] Feature feature)
        {
            if (db.Feature.Select(n => n).Where(c => c.IdCar == feature.IdCar).Where(d => d.Type == feature.Type).Count() == 0)
            {
                if (ModelState.IsValid)
                {
                    db.Feature.Add(feature);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", feature.IdCar);
            return View(feature);
        }
    }
}
