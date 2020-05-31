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
    [Authorize(Roles = "Admin,Seller,Manager")]
    public class OrdersController : Controller
    {
        private Entities _db = new Entities();

        // GET: Orders
        public ActionResult Index()
        {
            var order = _db.Order.Include(o => o.AspNetUsers).Include(o => o.Car).Include(o => o.Person);

            return View(order.ToList());
        }

        [Authorize(Roles = "Admin,Seller,Manager")]
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _db.Order.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Admin,Seller")]
        // GET: Orders/Create
        public ActionResult Create()
        {
            Dictionary<int, string> status = new Dictionary<int, string>();//temp
            status.Add(1, "Ready");
            status.Add(2, "In progress");

            var statusList = status.Select(n => new
            {
                Id = n.Key,
                Value = n.Value
            });
            ViewBag.Status = new SelectList(statusList, "Id", "Value");//temp


            var customers = _db.Person.Select(c => new
            {
                Id = c.Id,
                Description = c.Name + " " + c.Surname
            }).ToList();

            var cars = _db.Car.Select(n => new
            {
                Id = n.Id,
                IdSold = n.IdSold,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).Where(c => c.IdSold == false).ToList();


            ViewBag.IdCustomer = new SelectList(customers, "Id", "Description");
            ViewBag.IdCar = new SelectList(cars, "Id", "Description");

            ViewBag.IdSeller = new SelectList(_db.AspNetUsers, "Id", "Email");
            //ViewBag.IdCar = new SelectList(_db.Car.Select(n => n).Where(c => c.IdSold == false), "Id", "Brand");
            //ViewBag.IdCustomer = new SelectList(_db.Person, "Id", "Name");

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

            if (order.Status == "1")//temp
            {
                order.Status = "Ready";
            }
            else if (order.Status == "2")
            {
                order.Status = "In progress";
            }//temp

            if (ModelState.IsValid)
            {
                _db.Order.Add(order);
                _db.SaveChanges();


                Car car = _db.Car.Find(order.IdCar);
                car.IdSold = true;

                _db.Entry(car).State = EntityState.Modified;
                _db.SaveChanges();


                return RedirectToAction("Index");
            }

            Dictionary<int, string> status = new Dictionary<int, string>();//temp
            status.Add(1, "Ready");
            status.Add(2, "In progress");

            var statusList = status.Select(n => new
            {
                Id = n.Key,
                Value = n.Value
            });
            ViewBag.Status = new SelectList(statusList, "Id", "Value");//temp

            var customers = _db.Person.Select(c => new
            {
                Id = c.Id,
                Description = c.Name + " " + c.Surname
            }).ToList();

            var cars = _db.Car.Select(n => new
            {
                Id = n.Id,
                IdSold = n.IdSold,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).Where(c => c.IdSold == false).ToList();


            ViewBag.IdCustomer = new SelectList(customers, "Id", "Description");
            ViewBag.IdCar = new SelectList(cars, "Id", "Description");

            ViewBag.IdSeller = new SelectList(_db.AspNetUsers, "Id", "Email");

            return View(order);
        }

        [Authorize(Roles = "Admin,Seller")]
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _db.Order.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            var customers = _db.Person.Select(c => new
            {
                Id = c.Id,
                Description = c.Name + " " + c.Surname
            }).ToList();

            var cars = _db.Car.Select(n => new
            {
                Id = n.Id,
                IdSold = n.IdSold,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).Where(c => c.IdSold == false).ToList();


            ViewBag.IdCustomer = new SelectList(customers, "Id", "Description");
            ViewBag.IdCar = new SelectList(cars, "Id", "Description");

            ViewBag.IdSeller = new SelectList(_db.AspNetUsers, "Id", "Email", order.IdSeller);
            //ViewBag.IdCar = new SelectList(_db.Car, "Id", "Brand", order.IdCar);
            //ViewBag.IdCustomer = new SelectList(_db.Person, "Id", "Name", order.IdCustomer);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCar,IdCustomer,IdSeller,Status,Date")] Order order)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSeller = new SelectList(_db.AspNetUsers, "Id", "Email", order.IdSeller);
            ViewBag.IdCar = new SelectList(_db.Car, "Id", "Brand", order.IdCar);
            ViewBag.IdCustomer = new SelectList(_db.Person, "Id", "Name", order.IdCustomer);

            return View(order);
        }

        [Authorize(Roles = "Admin,Seller")]
        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _db.Order.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = _db.Order.Find(id);

            _db.Order.Remove(order);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
