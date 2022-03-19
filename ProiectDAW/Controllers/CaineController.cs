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
using PagedList;

namespace ProiectDAW.Controllers
{
    [Authorize]
    public class CaineController : Controller
    {
        private Entities db = new Entities();

        // GET: Caine
        public ActionResult Index()
        {
            //var caines = db.Caines.Include(c => c.Adoptie).Include(c => c.AspNetUser);
            return View(db.Caines.ToList());
        }

        public ActionResult Paginare(int? pagina,string disp)
        {
            var caini = from c in db.Caines
                        select c;
            if (disp == null)
            {
                caini = from c in db.Caines
                            select c;
            }
            else if (disp=="True")
            {
                //Boolean disp = ViewBag.Disponibili;
                caini = from c in db.Caines
                        where c.Disponibil == true
                        select c;
            }
            else
            {
                caini = from c in db.Caines
                        where c.Disponibil == false
                        select c;
            }
            return View(caini.ToList().ToPagedList(pagina ?? 1,5));
        }

        //POST: Caine/Paginare
        [HttpPost]
        public ActionResult Paginare(int? pagina,int? reset, FormCollection formCollection)
        {
            Boolean disp = false;
            if (!string.IsNullOrEmpty(formCollection["dispchk"])) { disp = true; }
            ViewBag.Disponibili = disp;
            var caini = from c in db.Caines
                        where c.Disponibil == disp
                        select c;
            if (reset != null)
            {
                caini = from c in db.Caines
                        select c;
            }
            return RedirectToAction("Paginare",new { pagina=pagina, disp=disp});
        }

        // GET: Caine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caine caine = db.Caines.Find(id);
            if (caine == null)
            {
                return HttpNotFound();
            }
            CaineViewModel cvm = new CaineViewModel()
            {
                caine=caine,
                adoptie = db.Adopties.Where(a => a.CaineId == caine.CaineId).OrderBy(p => p.DataAdoptie).FirstOrDefault()
            };
            if (cvm.adoptie==null)
            {
                ViewBag.goala = "1";
            }
            return View(cvm);
        }

        // GET: Caine/Create
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Caine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CaineId,Nume,Cod,Gen,DataNasterii,Disponibil,UserId,UpdateDate")] Caine caine)
        {
            caine.UpdateDate = DateTime.Now;
            caine.UserId = User.Identity.GetUserId();
            caine.Disponibil = true;
            if (ModelState.IsValid)
            {

                if (db.Caines.Any(r => r.Nume == caine.Nume))
                {
                    // exista un caine cu numele
                    ModelState.AddModelError("nume", "Există deja un câine cu acest nume!");
                    ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
                    return View(caine);
                }
                if (db.Caines.Any(r => r.Cod == caine.Cod))
                {
                    // exista un caine cu codul
                    ModelState.AddModelError("cod", "Există deja un câine cu acest cod!");
                    ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
                    return View(caine);
                }
                db.Caines.Add(caine);
                db.SaveChanges();
                return RedirectToAction("Paginare");
            }
            ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
            //ViewBag.CaineId = new SelectList(db.Adopties, "AdoptieId", "NrInregistrare", caine.CaineId);
            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", caine.UserId);
            return View(caine);
        }

        // GET: Caine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caine caine = db.Caines.Find(id);
            if (caine == null)
            {
                return HttpNotFound();
            }
            ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
            return View(caine);
        }

        // POST: Caine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaineId,Nume,Cod,Gen,DataNasterii,Disponibil,UserId,UpdateDate")] Caine caine)
        {
            caine.UpdateDate = DateTime.Now;
            caine.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (db.Caines.Any(r => r.Nume == caine.Nume && r.CaineId!=caine.CaineId))
                {
                    // exista un caine cu numele
                    ModelState.AddModelError("nume", "Există deja un câine cu acest nume!");
                    ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
                    return View(caine);
                }
                if (db.Caines.Any(r => r.Cod == caine.Cod && r.CaineId != caine.CaineId))
                {
                    ModelState.AddModelError("cod", "Există deja un câine cu acest cod!");
                    ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
                    return View(caine);
                }
                db.Entry(caine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Paginare");
            }
            ViewBag.DataNasterii = caine.DataNasterii.ToString("yyyy-MM-dd");
            return View(caine);
        }

        // GET: Caine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caine caine = db.Caines.Find(id);
            if (caine == null)
            {
                return HttpNotFound();
            }
            return View(caine);
        }

        // POST: Caine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Caine caine = db.Caines.Find(id);
            db.Caines.Remove(caine);
            db.SaveChanges();
            return RedirectToAction("Paginare");
        }

        // GET: Caine/AdaugaAdoptie/id
        public ActionResult AdaugaAdoptie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caine caine = db.Caines.Find(id);
            if (caine == null)
            {
                return HttpNotFound();
            }
            ViewBag.caine = caine;
            var lp = (from p in db.Persoanas
                      orderby p.Nume, p.Prenume
                      select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet");

            //ViewBag.CaineId = new SelectList(db.Caines.Where(c => c.Disponibil == true).OrderBy(l => l.Nume).ToList(), "CaineId", "Nume");
            return View();
        }

        // POST: Caine/AdaugaAdoptie/id
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
            Caine caine = db.Caines.Find(id);
            if (caine == null)
            {
                return HttpNotFound();
            }
            ViewBag.caine = caine;
            adoptie.CaineId = caine.CaineId;
            
                if (db.Adopties.Any(r => r.NrInregistrare == adoptie.NrInregistrare))
                {
                    ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
                var lp = (from p in db.Persoanas
                          orderby p.Nume, p.Prenume
                          select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
                ViewBag.PersoanaId = new SelectList(lp, "PersoanaId", "NumeComplet");
                ModelState.AddModelError("NrInregistrare", "Există deja o adopție cu acest număr de înregistrare!");
                    return View("AdaugaAdoptie", adoptie);
                }

                db.Adopties.Add(adoptie);
                caine.Disponibil = false;
                db.Entry(caine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = caine.CaineId });
            
            ViewBag.DataAdoptiei = adoptie.DataAdoptie.ToString("yyyy-MM-dd");
            var lp2 = (from p in db.Persoanas
                      orderby p.Nume, p.Prenume
                      select new { p.PersoanaId, NumeComplet = p.Nume + " " + p.Prenume }).ToList();
            ViewBag.PersoanaId = new SelectList(lp2, "PersoanaId", "NumeComplet");
            return View("AdaugaAdoptie", adoptie);
        }

        // POST: Caine/Lista
        [HttpPost]
        public JsonResult Lista()
        {
            var caines = db.Caines.Where(c => c.Disponibil == true).Select(s => new { s.CaineId, s.Nume });
            return Json(caines);
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
