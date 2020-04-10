using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BD_CDMS.Models;
using Microsoft.AspNet.Identity;

namespace BD_CDMS.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private Entities db = new Entities();

        // GET: Orders
        public ActionResult Index()
        {
            var order = db.Order.Include(o => o.AspNetUsers).Include(o => o.Car).Include(o => o.Person);
            return View(order.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.IdSeller = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand");
            ViewBag.IdCustomer = new SelectList(db.Person, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCar,IdCustomer,IdSeller,Status,Date")] Order order)
        {
            order.IdSeller = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            order.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();


                Car car = db.Car.Find(order.IdCar);
                car.IdSold = true;

                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            ViewBag.IdSeller = new SelectList(db.AspNetUsers, "Id", "Email", order.IdSeller);
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", order.IdCar);
            ViewBag.IdCustomer = new SelectList(db.Person, "Id", "Name", order.IdCustomer);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSeller = new SelectList(db.AspNetUsers, "Id", "Email", order.IdSeller);
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", order.IdCar);
            ViewBag.IdCustomer = new SelectList(db.Person, "Id", "Name", order.IdCustomer);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,IdCar,IdCustomer,IdSeller,Status,Date")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSeller = new SelectList(db.AspNetUsers, "Id", "Email", order.IdSeller);
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", order.IdCar);
            ViewBag.IdCustomer = new SelectList(db.Person, "Id", "Name", order.IdCustomer);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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
