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
    [Authorize(Roles = "Administrator, Operator")]
    public class JudetController : Controller
    {
        private Entities db = new Entities();

        // GET: Judet
        public ActionResult Index()
        {
            return View(db.Judets.ToList());
        }

        // GET: Judet/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Judet judet = db.Judets.Find(id);
            if (judet == null)
            {
                return HttpNotFound();
            }
            return View(judet);
        }

        // GET: Judet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Judet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JudetID,Denumire")] Judet judet)
        {
            if (ModelState.IsValid)
            {
                if (db.Judets.Any(r => r.Denumire == judet.Denumire))
                {
                    ModelState.AddModelError("Denumire", "Există deja un județ cu acestă denumire!");
                    return View(judet);
                }
                if (db.Judets.Any(r => r.JudetID == judet.JudetID))
                {
                    ModelState.AddModelError("JudetID", "Există deja un județ cu acest indicativ!");
                    return View(judet);
                }
                db.Judets.Add(judet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(judet);
        }

        // POST: Judet/AdaugaLocalitate
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaugaLocalitate(string id,[Bind(Include = "LocalitateID,Denumire,JudetID")] Localitate localitate)
        {
            Judet judet = db.Judets.Find(id);
            if (ModelState.IsValid)
            {
                if (db.Localitates.Any(r => r.Denumire == localitate.Denumire & r.JudetID==id))
                {
                    ModelState.AddModelError("localitate.Denumire", "Există deja o localitate cu această denumire în județul "+id+"!");
                    JudetViewModel jvm0 = new JudetViewModel();
                    jvm0.judet = judet; // judet invalid
                    jvm0.localitati = db.Localitates.Where(j => j.JudetID == judet.JudetID).OrderBy(l => l.Denumire).ToList();
                    jvm0.localitate = localitate;
                    TempData["AdaugaLocalitate"] = "1";
                    return View("Edit", jvm0);
                }
                db.Localitates.Add(localitate);
                db.SaveChanges();
                return RedirectToAction("Edit","Judet",new { id=id});
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            judet = db.Judets.Find(id);
            if (judet == null)
            {
                return HttpNotFound();
            }
            JudetViewModel jvm = new JudetViewModel();
            jvm.judet = judet; // judet invalid
            jvm.localitati = db.Localitates.Where(j => j.JudetID == judet.JudetID).OrderBy(l => l.Denumire).ToList();
            jvm.localitate = localitate;
            TempData["AdaugaLocalitate"] = "1";
            return View("Edit",jvm);
        }

        // GET: Judet/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Judet judet = db.Judets.Find(id);
            if (judet == null)
            {
                return HttpNotFound();
            }
            JudetViewModel jvm = new JudetViewModel();
            jvm.judet = judet;
            jvm.localitati = db.Localitates.Where(j => j.JudetID == judet.JudetID).OrderBy(l => l.Denumire).ToList();
            jvm.localitate = new Localitate() { JudetID = judet.JudetID };
            return View(jvm);
        }

        // POST: Judet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,[Bind(Include = "JudetID,Denumire")] Judet judet)
        {
            if (ModelState.IsValid)
            {
                if (db.Judets.Any(r => r.Denumire == judet.Denumire && r.JudetID != judet.JudetID))
                {
                    ModelState.AddModelError("Denumire", "Există deja un județ cu acest nume!");
                    return View(judet);
                }
                db.Entry(judet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            JudetViewModel jvm = new JudetViewModel();
            jvm.judet = judet; // judet invalid
            jvm.localitati = db.Localitates.Where(j => j.JudetID == judet.JudetID).OrderBy(l => l.Denumire).ToList();
            jvm.localitate = new Localitate() { JudetID = judet.JudetID };
            TempData["EditJudet"] = "1";
            return View(jvm);
        }

        // GET: Judet/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Judet judet = db.Judets.Find(id);
            if (judet == null)
            {
                return HttpNotFound();
            }
            return View(judet);
        }

        // POST: Judet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Judet judet = db.Judets.Find(id);
            db.Judets.Remove(judet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Judet/EditareLocalitate/id
        public ActionResult EditareLocalitate(int? id)
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

        // POST: Judet/EditareLocalitate/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditareLocalitate([Bind(Include = "LocalitateID,Denumire,JudetID")] Localitate localitate)
        {
            if (ModelState.IsValid)
            {
                if (db.Localitates.Any(r => r.Denumire == localitate.Denumire & r.JudetID == localitate.JudetID & r.LocalitateID!=localitate.LocalitateID))
                {
                    ModelState.AddModelError("Denumire", "Există deja o localitate cu această denumire în județul " + localitate.JudetID + "!");
                    ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire", localitate.JudetID);
                    return View(localitate);
                }
                db.Entry(localitate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Judet", new { id = localitate.JudetID });
            }
            ViewBag.JudetID = new SelectList(db.Judets, "JudetID", "Denumire", localitate.JudetID);
            return View(localitate);
        }

        // GET: Judet/StergeLocalitate/id
        public ActionResult StergeLocalitate(int? id)
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

        // POST: Judet/StergeLocalitate/id
        [HttpPost, ActionName("StergeLocalitate")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedLocalitate(int id)
        {
            Localitate localitate = db.Localitates.Find(id);
            string judetId = localitate.JudetID;
            db.Localitates.Remove(localitate);
            db.SaveChanges();
            return RedirectToAction("Edit", "Judet", new { id = judetId });
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
