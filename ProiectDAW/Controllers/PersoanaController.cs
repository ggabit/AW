using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProiectDAW.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Diagnostics;

namespace ProiectDAW.Controllers
{
    [Authorize]
    public class PersoanaController : Controller
    {
        private Entities db = new Entities();

        // GET: Persoana
        public ActionResult Index()
        {
            ViewBag.DataCurenta = DateTime.Now.ToString("d MMMM yyyy, HH:mm", new CultureInfo("ro"));
            var lp = (from a in db.Adopties
                      select new { a.PersoanaId }).ToList();
            ViewBag.PersoanaId = new SelectList(lp, "PersoanaId");
            return View(db.Persoanas.ToList());
        }

        // GET: Persoana/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            PersoanaViewModel pvm = new PersoanaViewModel()
            {
                persoana = persoana,
                detaliu = db.PersoanaDetalius.Find(id),
                adoptii=db.Adopties.Where(a => a.PersoanaId == persoana.PersoanaId).OrderBy(p => p.DataAdoptie).ToList(),
                adoptie = new Adoptie() { PersoanaId = persoana.PersoanaId }
            };
            if (!pvm.adoptii.Any())
            {
                ViewBag.goala = "1";
            }
            return View(pvm);
        }

        // GET: Persoana/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Persoana/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersoanaId,Nume,Prenume,CNP,DataNasterii,NrTelefon,UserId,UpdateDate")] Persoana persoana)
        {
            persoana.UpdateDate = DateTime.Now;
            persoana.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (db.Persoanas.Any(r => r.CNP == persoana.CNP))
                {
                    // exista o persoană cu CNP
                    ViewBag.DataNasterii = persoana.DataNasterii.ToString("yyyy-MM-dd");
                    ModelState.AddModelError("CNP", "Există deja o persoană cu acest CNP!");
                    return View(persoana);
                }
                db.Persoanas.Add(persoana);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DataNasterii = persoana.DataNasterii.ToString("yyyy-MM-dd");
            return View(persoana);
        }

        // GET: Persoana/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            ViewBag.DataNasterii = persoana.DataNasterii.ToString("yyyy-MM-dd");
            PersoanaAdoptiiViewModel pavm = new PersoanaAdoptiiViewModel()
            {
                persoana = persoana,
                adoptii = db.Adopties.Where(a => a.PersoanaId == persoana.PersoanaId).OrderBy(p => p.DataAdoptie).ToList(),
                adoptie = new Adoptie() { PersoanaId = persoana.PersoanaId }
            };
            
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", pavm.adoptie.CaineId);
            //ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume");
            return View(pavm);
        }

        // POST: Persoana/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersoanaId,Nume,Prenume,CNP,DataNasterii,NrTelefon,UserId,UpdateDate")] Persoana persoana)
        {
            persoana.UpdateDate = DateTime.Now;
            persoana.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (db.Persoanas.Any(r => r.CNP == persoana.CNP && r.PersoanaId != persoana.PersoanaId))
                {
                    ViewBag.DataNasterii = persoana.DataNasterii.ToString("yyyy-MM-dd");
                    ModelState.AddModelError("CNP", "Există deja o persoană cu acest CNP!");
                    return View(persoana);
                }
                db.Entry(persoana).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DataNasterii = persoana.DataNasterii.ToString("yyyy-MM-dd");
            PersoanaAdoptiiViewModel pavm = new PersoanaAdoptiiViewModel();
            pavm.persoana = persoana; // judet invalid
            pavm.adoptii = db.Adopties.Where(p => p.PersoanaId == persoana.PersoanaId).OrderBy(a => a.DataAdoptie).ToList();
            pavm.adoptie = new Adoptie() { PersoanaId = persoana.PersoanaId };
            TempData["EditPersoana"] = "1";
            return View(pavm);
        }

        // GET: Persoana/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            var lp = (from a in db.Adopties
                      where a.PersoanaId==persoana.PersoanaId
                      select new { a.PersoanaId }).ToList();
            if (lp.Any())
            {
                ViewBag.Stergere = null;
                ViewBag.StergereTitlu = "Această persoană are adopții înregistrate. Nu se poate șterge.";
            }
            else
            {
                ViewBag.Stergere = "1";
                ViewBag.StergereTitlu = "Confirmați ștergerea?";
            }
            PersoanaViewModel pvm = new PersoanaViewModel
            {
                persoana = persoana,
                adoptii= db.Adopties.Where(p => p.PersoanaId == persoana.PersoanaId).OrderBy(a => a.DataAdoptie).ToList()
            };
            return View(pvm);
        }

        // POST: Persoana/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persoana persoana = db.Persoanas.Find(id);
            db.Persoanas.Remove(persoana);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Persoana/AdaugaDetaliu
        public ActionResult AdaugaDetaliu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            ViewBag.persoana = persoana;
            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume");
            /*var lj = (from l in db.Localitates
                      join j in db.Judets on l.JudetID equals j.JudetID
                      orderby j.Denumire, l.Denumire
                      select new { l.LocalitateID, LocalitateJudet = l.Denumire + " (" + j.Denumire + ") " }).ToList();*/
            PersoanaDetaliu pd = db.PersoanaDetalius.Find(id);
            if (pd == null)
            {
                ViewBag.JudetID = new SelectList(db.Judets.OrderBy(j => j.Denumire), "JudetID", "Denumire");
                string jID = db.Judets.OrderBy(j => j.Denumire).FirstOrDefault().JudetID;
                ViewBag.LocalitateID = new SelectList(db.Localitates.Where(j => j.JudetID == jID), "LocalitateID", "Denumire");
                pd = new PersoanaDetaliu() { PersoanaId = id.Value };
                TempData["btnText"] = "Adaugă detalii";
                TempData["btnCSS"] = "success";
            }
            else
            {
                // le afisam
                string jID = db.Localitates.Find(pd.LocalitateID).JudetID;
                ViewBag.JudetID = new SelectList(db.Judets.OrderBy(j => j.Denumire), "JudetID", "Denumire", jID);
                ViewBag.LocalitateID = new SelectList(db.Localitates.Where(j => j.JudetID == jID), "LocalitateID", "Denumire", pd.LocalitateID);
                TempData["btnText"] = "Actualizare";
                TempData["btnCSS"] = "warning";
            }
            return View(pd);
        }

        // POST: Persoana/AdaugaDetaliu
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaugaDetaliu(int? id,[Bind(Include = "PersoanaId,Adresa,LocuintaCurte,LocalitateID")] PersoanaDetaliu persoanaDetaliu, string JudetID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            ViewBag.persoana = persoana;
            PersoanaDetaliu pd = db.PersoanaDetalius.Find(id);
            if (pd == null)
            {
                db.PersoanaDetalius.Add(persoanaDetaliu);
                TempData["btnText"] = "Adaugă detalii";
                TempData["btnCSS"] = "success";
            }
            else
            {
                pd.Adresa = persoanaDetaliu.Adresa;
                pd.LocuintaCurte = persoanaDetaliu.LocuintaCurte;
                pd.LocalitateID = persoanaDetaliu.LocalitateID;
                db.Entry(pd).State = EntityState.Modified;
                TempData["btnText"] = "Actualizare";
                TempData["btnCSS"] = "primary";
            }
            if (ModelState.IsValid)
            {
                string b= TempData["btnText"].ToString();
                b=TempData["btnCSS"].ToString();
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume", persoanaDetaliu.PersoanaId);
            string jID = db.Localitates.Find(persoanaDetaliu.LocalitateID).JudetID;
            ViewBag.JudetID = new SelectList(db.Judets.OrderBy(j => j.Denumire), "JudetID", "Denumire", jID);
            ViewBag.LocalitateID = new SelectList(db.Localitates, "LocalitateID", "Denumire", persoanaDetaliu.LocalitateID);
            
            return View(persoanaDetaliu);
        }

        // GET: Persoana/AdaugaAdoptie/id
        public ActionResult AdaugaAdoptie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            ViewBag.persoana = persoana;
            //ViewBag.PersoanaId = new SelectList(db.Persoanas, "PersoanaId", "Nume");

            ViewBag.CaineId = new SelectList(db.Caines.Where(c => c.Disponibil == true).OrderBy(l => l.Nume).ToList(), "CaineId", "Nume");
            return View();
        }

        // POST: Persoana/AdaugaAdoptie/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdaugaAdoptie(int? id, [Bind(Include = "AdoptieId,NrInregistrare,CaineId,PersoanaId,DataAdoptie,UserId,UpdateDate")] Adoptie adoptie)
        {
            adoptie.UpdateDate = DateTime.Now;
            adoptie.UserId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persoana persoana = db.Persoanas.Find(id);
            if (persoana == null)
            {
                return HttpNotFound();
            }
            ViewBag.persoana = persoana;
            adoptie.Persoana = persoana;
            adoptie.PersoanaId = persoana.PersoanaId;
                if (db.Adopties.Any(r => r.NrInregistrare == adoptie.NrInregistrare))
                {

                    Debug.Write("Duplicat");
                    ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
                    ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", "CaineId");
                    ModelState.AddModelError("NrInregistrare", "Există deja o adopție cu acest număr de înregistrare!");
                    return View("AdaugaAdoptie",adoptie);
                }
                Debug.Write("E valid, il salvez");
                db.Adopties.Add(adoptie);
                Caine caine = db.Caines.Find(adoptie.CaineId);
                caine.Disponibil = false;
                db.Entry(caine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit",new { id=persoana.PersoanaId});
            
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach(var e in errors)
            {
                Debug.Write("Nu e valid, return cu erorile " + e.ToString());
            }
            ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", "CaineId");
            return View("AdaugaAdoptie", adoptie);
        }

        [HttpPost]
        public PartialViewResult AdoptiilePersoanei(int? PersoanaId)
        {
            return PartialView("_AdoptiilePersoanei",db.Adopties.Where(p=>p.PersoanaId==PersoanaId).OrderBy(a => a.DataAdoptie));
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
