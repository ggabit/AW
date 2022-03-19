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
    public class PersoanaDetaliuController : Controller
    {
        private Entities db = new Entities();

        // GET: PersoanaDetaliu
        public ActionResult Index()
        {
            var persoanaDetalius = db.PersoanaDetalius.Include(p => p.Persoana).Include(p => p.Localitate);
            return View(persoanaDetalius.ToList());
        }

        // GET: PersoanaDetaliu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersoanaDetaliu persoanaDetaliu = db.PersoanaDetalius.Find(id);
            if (persoanaDetaliu == null)
            {
                return HttpNotFound();
            }
            return View(persoanaDetaliu);
        }

        // GET: PersoanaDetaliu/Create
        public ActionResult Create()
        {
            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume");
            var lj = (from l in db.Localitates
                      join j in db.Judets on l.JudetID equals j.JudetID
                      orderby j.Denumire, l.Denumire
                      select new { l.LocalitateID, LocalitateJudet = l.Denumire + " (" + j.Denumire + ") " }).ToList();
            ViewBag.LocalitateID = new SelectList(lj, "LocalitateID", "LocalitateJudet");
            return View();
        }

        // POST: PersoanaDetaliu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersoanaId,Adresa,LocuintaCurte,LocalitateID")] PersoanaDetaliu persoanaDetaliu)
        {
            if (ModelState.IsValid)
            {
                db.PersoanaDetalius.Add(persoanaDetaliu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume", persoanaDetaliu.PersoanaId);
            ViewBag.LocalitateID = new SelectList(db.Localitates, "LocalitateID", "Denumire", persoanaDetaliu.LocalitateID);
            return View(persoanaDetaliu);
        }

        // GET: PersoanaDetaliu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersoanaDetaliu persoanaDetaliu = db.PersoanaDetalius.Find(id);
            if (persoanaDetaliu == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume", persoanaDetaliu.PersoanaId);
            ViewBag.LocalitateID = new SelectList(db.Localitates, "LocalitateID", "Denumire", persoanaDetaliu.LocalitateID);
            return View(persoanaDetaliu);
        }

        // POST: PersoanaDetaliu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersoanaId,Adresa,LocuintaCurte,LocalitateID")] PersoanaDetaliu persoanaDetaliu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persoanaDetaliu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume", persoanaDetaliu.PersoanaId);
            ViewBag.LocalitateID = new SelectList(db.Localitates, "LocalitateID", "Denumire", persoanaDetaliu.LocalitateID);
            return View(persoanaDetaliu);
        }

        // GET: PersoanaDetaliu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersoanaDetaliu persoanaDetaliu = db.PersoanaDetalius.Find(id);
            if (persoanaDetaliu == null)
            {
                return HttpNotFound();
            }
            return View(persoanaDetaliu);
        }

        // POST: PersoanaDetaliu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersoanaDetaliu persoanaDetaliu = db.PersoanaDetalius.Find(id);
            db.PersoanaDetalius.Remove(persoanaDetaliu);
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
