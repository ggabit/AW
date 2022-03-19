using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    [Authorize]
    public class UserDetaliuController : Controller
    {
        private Entities db = new Entities();

        // GET: UserDetaliu
        public ActionResult Index()
        {
            return View(db.UserDetalius.ToList());
        }

        // GET: UserDetaliu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetaliu userDetaliu = db.UserDetalius.Find(id);
            if (userDetaliu == null)
            {
                return HttpNotFound();
            }
            return View(userDetaliu);
        }

        // GET: UserDetaliu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetaliu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Nume,Prenume,DataNastere")] UserDetaliu userDetaliu)
        {
            if (ModelState.IsValid)
            {
                db.UserDetalius.Add(userDetaliu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetaliu);
        }

        // GET: UserDetaliu/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetaliu userDetaliu = db.UserDetalius.Find(id);
            if (userDetaliu == null)
            {
                return HttpNotFound();
            }
            return View(userDetaliu);
        }

        // POST: UserDetaliu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Nume,Prenume,DataNastere")] UserDetaliu userDetaliu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDetaliu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetaliu);
        }

        // GET: UserDetaliu/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetaliu userDetaliu = db.UserDetalius.Find(id);
            if (userDetaliu == null)
            {
                return HttpNotFound();
            }
            return View(userDetaliu);
        }

        // POST: UserDetaliu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetaliu userDetaliu = db.UserDetalius.Find(id);
            db.UserDetalius.Remove(userDetaliu);
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
