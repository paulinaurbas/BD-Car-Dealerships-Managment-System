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
    public class DealershipSalonController : Controller
    {
        private Entities db = new Entities();

        [Authorize(Roles = "Admin,Seller,Serviceman")]
        // GET: DealershipSalon
        public ActionResult Index(string searchSalon, string searchColor, string searchBrand, string searchModel, int? searchGearbox, int? searchEngine, int? searchCarType,
            decimal? searchMinPrice, decimal? searchMaxPrice)
        {
            //var car = db.Car.Include(c => c.CarType).Include(c => c.DealershipSalon).Include(c => c.Engine).Include(c => c.Gearbox);
            //return View(car.ToList());

            ViewBag.searchMinPrice = searchMinPrice;
            ViewBag.searchMaxPrice = searchMaxPrice;

            ViewBag.searchCarType = new SelectList(db.CarType, "Id", "Type");
            ViewBag.searchEngine = new SelectList(db.Engine, "Id", "Type");
            ViewBag.searchGearbox = new SelectList(db.Gearbox, "Id", "Type");

            if (searchSalon != null)
            {
                var car = db.Car.Select(n => n).
                    Where(c => c.IdSold == false).
                    Where(d => d.DealershipSalon.Name.Contains(searchSalon)).
                    Where(d => d.Color.Contains(searchColor)).
                    Where(d => d.Brand.Contains(searchBrand)).
                    Where(d => d.Model.Contains(searchModel));

                if (searchGearbox != null)
                {
                    car = car.Where(d => d.Gearbox.Id == searchGearbox);
                }
                if (searchEngine != null)
                {
                    car = car.Where(d => d.Engine.Id == searchEngine);
                }
                if (searchCarType != null)
                {
                    car = car.Where(d => d.CarType.Id == searchCarType);
                }

                if (searchMinPrice != null)
                {
                    car = car.Where(d => d.Price >= searchMinPrice);
                }
                if (searchMaxPrice != null)
                {
                    car = car.Where(d => d.Price <= searchMaxPrice);
                }

                if (Request.IsAjaxRequest())
                    return PartialView("SearchCarList", car.ToList());

                return View(car.ToList());
            }
            else
            {
                var car = db.Car.Select(n => n).Where(c => c.IdSold == false);

                return View(car.ToList());
            }
        }

        [Authorize(Roles = "Admin,Seller,Serviceman")]
        // GET: DealershipSalon/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "Admin")]
        // GET: DealershipSalon/Create
        public ActionResult Create()
        {
            ViewBag.IdCarType = new SelectList(db.CarType, "Id", "Type");
            ViewBag.IdEngine = new SelectList(db.Engine, "Id", "Type");
            ViewBag.IdGearbox = new SelectList(db.Gearbox, "Id", "Type");

            var dealershipSalons = db.DealershipSalon.Select(n => new
            {
                Id = n.Id,
                Description = n.Name
            }).ToList();

            ViewBag.IdDealershipSalon = new SelectList(dealershipSalons, "Id", "Description");

            return View();
        }

        // POST: DealershipSalon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Brand,Model,VIN,Color,IdDealershipSalon,ImagePath,IdSold,Price,HP,IdCarType,IdEngine,IdGearbox")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Car.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCarType = new SelectList(db.CarType, "Id", "Type", car.IdCarType);
            ViewBag.IdEngine = new SelectList(db.Engine, "Id", "Type", car.IdEngine);
            ViewBag.IdGearbox = new SelectList(db.Gearbox, "Id", "Type", car.IdGearbox);

            var dealershipSalons = db.DealershipSalon.Select(n => new
            {
                Id = n.Id,
                Description = n.Name
            }).ToList();

            ViewBag.IdDealershipSalon = new SelectList(dealershipSalons, "Id", "Description");

            return View(car);
        }

        [Authorize(Roles = "Admin")]
        // GET: DealershipSalon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCarType = new SelectList(db.CarType, "Id", "Type", car.IdCarType);
            ViewBag.IdEngine = new SelectList(db.Engine, "Id", "Type", car.IdEngine);
            ViewBag.IdGearbox = new SelectList(db.Gearbox, "Id", "Type", car.IdGearbox);

            var dealershipSalons = db.DealershipSalon.Select(n => new
            {
                Id = n.Id,
                Description = n.Name
            }).ToList();

            ViewBag.IdDealershipSalon = new SelectList(dealershipSalons, "Id", "Description");

            return View(car);
        }

        // POST: DealershipSalon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Brand,Model,VIN,Color,IdDealershipSalon,ImagePath,IdSold,Price,HP,IdCarType,IdEngine,IdGearbox")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCarType = new SelectList(db.CarType, "Id", "Type", car.IdCarType);
            ViewBag.IdEngine = new SelectList(db.Engine, "Id", "Type", car.IdEngine);
            ViewBag.IdGearbox = new SelectList(db.Gearbox, "Id", "Type", car.IdGearbox);

            var dealershipSalons = db.DealershipSalon.Select(n => new
            {
                Id = n.Id,
                Description = n.Name
            }).ToList();

            ViewBag.IdDealershipSalon = new SelectList(dealershipSalons, "Id", "Description");

            return View(car);
        }

        [Authorize(Roles = "Admin")]
        // GET: DealershipSalon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Car.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "Admin")]
        // POST: DealershipSalon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Car.Find(id);
            db.Car.Remove(car);
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
    }
}
