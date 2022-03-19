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
    [Authorize(Roles = "Operator,Administrator")]
    public class LocalitateController : Controller
    {
        private Entities db = new Entities();

        // GET: Localitate
        public ActionResult Index()
        {
            var localitates = db.Localitates.Include(l => l.Judet);
            return View(localitates.ToList());
        }

        // POST: Localitate/Lista
        [HttpPost]
        public JsonResult Lista(string judetID)
        {
            //string judetID = Request.Form["JudetID"].ToString();
            var localitates = db.Localitates.Where(j => j.JudetID == judetID).Select(s => new { s.LocalitateID, s.Denumire });
            return Json(localitates);
        }

        // GET: Localitate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitates.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            return View(localitate);
        }

        // GET: Localitate/Create
        public ActionResult Create()
        {
            ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire");
            return View();
        }

        // POST: Localitate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocalitateID,Denumire,JudetID")] Localitate localitate)
        {
            if (ModelState.IsValid)
            {
                db.Localitates.Add(localitate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire", localitate.JudetID);
            return View(localitate);
        }

        // GET: Localitate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitates.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire", localitate.JudetID);
            return View(localitate);
        }

        // POST: Localitate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocalitateID,Denumire,JudetID")] Localitate localitate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localitate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire", localitate.JudetID);
            return View(localitate);
        }

        // GET: Localitate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localitate localitate = db.Localitates.Find(id);
            if (localitate == null)
            {
                return HttpNotFound();
            }
            return View(localitate);
        }

        // POST: Localitate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Localitate localitate = db.Localitates.Find(id);
            db.Localitates.Remove(localitate);
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
