using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    [Authorize]
    public class AdoptieController : Controller
    {
        private Entities db = new Entities();

        // GET: Adoptie
        public ActionResult Index()
        {
            var adopties = db.Adopties.Include(a => a.AspNetUser).Include(a => a.Persoana).Include(a => a.Caine);
            return View(adopties.ToList());
        }

        // GET: Adoptie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adoptie adoptie = db.Adopties.Find(id);
            if (adoptie == null)
            {
                return HttpNotFound();
            }
            AdoptieViewModel avm = new AdoptieViewModel()
            {
                adoptie = adoptie,
                persoanaDetaliu = db.PersoanaDetalius.Where(pd => pd.PersoanaId == adoptie.PersoanaId).FirstOrDefault()
            };
            if (avm.persoanaDetaliu == null)
            {
                ViewBag.goala = "1";
            }
            return View(avm);
        }

        // GET: Adoptie/Create
        public ActionResult Create()
        {
            var lc = (from c in db.Caines
                      where c.Disponibil==true
                      select new { c.CaineId, c.Nume }).ToList();
            if (!lc.Any())
            {
                return RedirectToAction("Index","Caine");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            var lp = (from p in db.Persoanas
                      orderby p.Nume, p.Prenume
                      select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet");
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume");
            return View();
        }

        // POST: Adoptie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdoptieId,NrInregistrare,CaineId,PersoanaId,DataAdoptie,UserId,UpdateDate")] Adoptie adoptie)
        {
            adoptie.UpdateDate = DateTime.Now;
            adoptie.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (db.Adopties.Any(r => r.NrInregistrare == adoptie.NrInregistrare))
                {
                    ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
                    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", adoptie.UserId);
                    var lp = (from p in db.Persoanas
                               orderby p.Nume, p.Prenume
                               select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
                    ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet");
                    ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", adoptie.CaineId);
                    ModelState.AddModelError("NrInregistrare", "Există deja o adopție cu acest număr de înregistrare!");
                    return View(adoptie);
                }
                db.Adopties.Add(adoptie);
                Caine caine = db.Caines.Find(adoptie.CaineId);
                caine.Disponibil = false;
                db.Entry(caine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", adoptie.UserId);
            var lp2 = (from p in db.Persoanas
                       orderby p.Nume, p.Prenume
                       select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp2, "PersoanaId", "NumeComplet");
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", adoptie.CaineId);
            return View(adoptie);
        }

        // GET: Adoptie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adoptie adoptie = db.Adopties.Find(id);
            if (adoptie == null)
            {
                return HttpNotFound();
            }
            /*Caine caine = db.Caines.Find(adoptie.CaineId);
            caine.Disponibil = true;
            db.Entry(caine).State = EntityState.Modified;
            db.SaveChanges();*/
            ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
            TempData["caineVechi"] = adoptie.CaineId;
            ViewBag.id = adoptie.CaineId;
            ViewBag.nume = adoptie.Caine.Nume;
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", adoptie.UserId);
            var lp = (from p in db.Persoanas
                      orderby p.Nume, p.Prenume
                      select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet", adoptie.PersoanaId);
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", adoptie.CaineId);
            return View(adoptie);
        }

        // POST: Adoptie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdoptieId,NrInregistrare,CaineId,PersoanaId,DataAdoptie,UserId,UpdateDate")] Adoptie adoptie)
        {
            adoptie.UpdateDate = DateTime.Now;
            adoptie.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (db.Adopties.Any(r => r.NrInregistrare == adoptie.NrInregistrare && r.AdoptieId!=adoptie.AdoptieId))
                {
                    ViewBag.id = adoptie.CaineId;
                    Caine caine = db.Caines.Find(adoptie.CaineId);
                    ViewBag.nume = caine.Nume;
                    ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
                    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", adoptie.UserId);

                    var lp = (from p in db.Persoanas
                               orderby p.Nume, p.Prenume
                               select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
                    ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet");
                    ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", adoptie.CaineId);
                    ModelState.AddModelError("NrInregistrare", "Există deja o adopție cu acest număr de înregistrare!");
                    return View(adoptie);
                }
                if (adoptie.CaineId != (int) TempData["caineVechi"])
                {
                    Caine caine = db.Caines.Find(adoptie.CaineId);
                    caine.Disponibil = false;
                    db.Entry(caine).State = EntityState.Modified;
                    Caine caineV = db.Caines.Find((int)TempData["caineVechi"]);
                    caineV.Disponibil = true;
                    db.Entry(caineV).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.Entry(adoptie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = adoptie.CaineId;
            Caine cainee = db.Caines.Find(adoptie.CaineId);
            ViewBag.nume = cainee.Nume;
            ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", adoptie.UserId);
            var lp2 = (from p in db.Persoanas
                       orderby p.Nume, p.Prenume
                       select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp2, "PersoanaId", "NumeComplet");
            ViewBag.CaineId = new SelectList(db.Caines, "CaineId", "Nume", adoptie.CaineId);
            return View(adoptie);
        }

        // GET: Adoptie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adoptie adoptie = db.Adopties.Find(id);
            if (adoptie == null)
            {
                return HttpNotFound();
            }
            return View(adoptie);
        }

        // POST: Adoptie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adoptie adoptie = db.Adopties.Find(id);
            db.Adopties.Remove(adoptie);
            Caine caine = db.Caines.Find(adoptie.CaineId);
            caine.Disponibil = true;
            db.Entry(caine).State = EntityState.Modified;
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
