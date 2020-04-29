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
    public class IssuesController : Controller
    {
        private Entities db = new Entities();

        // GET: Issues
        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Index()
        {
            var issue = db.Issue.Include(i => i.AspNetUsers).Include(i => i.Car).Where(t => t.IdServiceman == null);
            return View(issue.ToList());
        }

        // GET: Issues/Details/5
        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // GET: Issues/Create
        [Authorize(Roles = "Admin,Serviceman,Seller")]
        public ActionResult Create()
        {
            var cars = db.Car.Select(n => new
            {
                Id = n.Id,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).ToList();

            ViewBag.IdCar = new SelectList(cars, "Id", "Description");

            ViewBag.IdServiceman = new SelectList(db.AspNetUsers, "Id", "Email");
            //ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand");
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCar,IdServiceman,Description,IsReady")] Issue issue)
        {
            issue.IsReady = false;

            if (ModelState.IsValid)
            {
                db.Issue.Add(issue);
                db.SaveChanges();

                if (System.Web.HttpContext.Current.User.IsInRole("Seller") && !System.Web.HttpContext.Current.User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "DealershipSalon");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.IdServiceman = new SelectList(db.AspNetUsers, "Id", "Email", issue.IdServiceman);
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", issue.IdCar);

            return View(issue);
        }

        // GET: Issues/Edit/5
        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            var cars = db.Car.Select(n => new
            {
                Id = n.Id,
                Description = n.Brand + " " + n.Model + " VIN: " + n.VIN
            }).ToList();

            ViewBag.IdCar = new SelectList(cars, "Id", "Description");
            ViewBag.IdServiceman = new SelectList(db.AspNetUsers, "Id", "Email", issue.IdServiceman);
            //ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", issue.IdCar);
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCar,IdServiceman,Description,IsReady")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdServiceman = new SelectList(db.AspNetUsers, "Id", "Email", issue.IdServiceman);
            ViewBag.IdCar = new SelectList(db.Car, "Id", "Brand", issue.IdCar);
            return View(issue);
        }

        // GET: Issues/Delete/5
        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }
            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Issue issue = db.Issue.Find(id);
            db.Issue.Remove(issue);
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

        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult IndexServiceman()
        {
            string user = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();

            var issue = db.Issue.
                Select(n => n).
                Where(b => b.IsReady == false).
                Where(t => t.IdServiceman == user);
            return View(issue.ToList());
        }

        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult IndexHistory()
        {
            var issue = db.Issue.Include(i => i.AspNetUsers).Include(i => i.Car).Where(b => b.IsReady == true).Where(t => t.IdServiceman != null);
            return View(issue.ToList());
        }

        // GET: Issues
        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult IndexAdmin()
        {
            var issue = db.Issue.Include(i => i.AspNetUsers).Include(i => i.Car);
            return View(issue.ToList());
        }


        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Reserve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            issue.IdServiceman = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();

            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("IndexSeviceman");
        }

        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Unreserve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            issue.IdServiceman = null;

            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("IndexSeviceman");
        }


        [Authorize(Roles = "Admin,Serviceman")]
        public ActionResult Ready(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Issue issue = db.Issue.Find(id);
            if (issue == null)
            {
                return HttpNotFound();
            }

            issue.IsReady = true;

            if (ModelState.IsValid)
            {
                db.Entry(issue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("IndexSeviceman");
        }
    }
}
