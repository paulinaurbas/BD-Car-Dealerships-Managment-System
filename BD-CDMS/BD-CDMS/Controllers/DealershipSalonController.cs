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
    [Authorize(Roles = "Admin,Seller")]
    public class DealershipSalonController : Controller
    {
        private Entities _db = new Entities();

        // GET: DealershipSalon
        public ActionResult Index()
        {
            var car = _db.Car.Select(n => n).Where(c => c.IdSold == false);

            return View(car.ToList());
        }

        // GET: DealershipSalon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = _db.Car.Find(id);

            if (car == null)
                return HttpNotFound();

            return View(car);
        }

        // GET: DealershipSalon/Create
        public ActionResult Create()
        {
            ViewBag.IdDealershipSalon = new SelectList(_db.DealershipSalon, "Id", "PhoneNumber");

            return View();
        }

        // POST: DealershipSalon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Brand,Model,VIN,Color,IdDealershipSalon,ImagePath")] Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Car.Add(car);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.IdDealershipSalon = new SelectList(_db.DealershipSalon, "Id", "PhoneNumber", car.IdDealershipSalon);

            return View(car);
        }

        // GET: DealershipSalon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = _db.Car.Find(id);

            if (car == null)
                return HttpNotFound();

            ViewBag.IdDealershipSalon = new SelectList(_db.DealershipSalon, "Id", "PhoneNumber", car.IdDealershipSalon);

            return View(car);
        }

        // POST: DealershipSalon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Brand,Model,VIN,Color,IdDealershipSalon,ImagePath")] Car car)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(car).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.IdDealershipSalon = new SelectList(_db.DealershipSalon, "Id", "PhoneNumber", car.IdDealershipSalon);

            return View(car);
        }

        // GET: DealershipSalon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Car car = _db.Car.Find(id);

            if (car == null)
                return HttpNotFound();

            return View(car);
        }

        // POST: DealershipSalon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = _db.Car.Find(id);

            _db.Car.Remove(car);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();

            base.Dispose(disposing);
        }



    }
}
