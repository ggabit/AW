using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class AdminController : Controller
    {
        // GET: Admin/Roluri
        public ActionResult Roluri()
        {
            var roluri = URM.getRoluri();
            return View(roluri);
        }

        // GET: Admin/Utilizatori
        public ActionResult Utilizatori()
        {
            var utilizatori = URM.getUseri();
            return View(utilizatori);
        }

        // GET: Admin/Rol/id
        public ActionResult Rol(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guid RolID;
            if (!Guid.TryParse(id, out RolID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rol = URM.getRol(RolID);
            if (rol == null)
            {
                return HttpNotFound();
            }
            List<ItemObject> lu = new List<ItemObject>();
            lu = URM.getUseri(RolID);
            rol.list = lu;
            return View(rol);
        }

        // GET: Admin/Utilizator/id
        public ActionResult Utilizator(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guid UtilizatorID;
            if (!Guid.TryParse(id, out UtilizatorID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var utilizator = URM.getUser(UtilizatorID);
            if (utilizator == null)
            {
                return HttpNotFound();
            }
            List<ItemObject> lr = new List<ItemObject>();
            lr = URM.getRoluri();
            var lra = URM.getRoluri(UtilizatorID);
            foreach(var rol in lr)
            {
                if (lra.Where(x => x.ItemID == rol.ItemID).ToList().Count > 0)
                {
                    rol.SelectedItem = true;
                }
                else
                {
                    rol.SelectedItem = false;
                }
            }
            utilizator.list = lr;
            if (Session["updateUser"].ToString() == "1")
            {
                if (Session["updateUser0"].ToString() == "1")
                {
                    ViewBag.MesajAlert = "Rolurile au fost actualizate!";
                    ViewBag.CSSAlert = " alert-success";
                }
                else
                {
                    ViewBag.MesajAlert = "Nu a fost nimic de actualizat!";
                    ViewBag.CSSAlert = " alert-info";
                }
            }
            return View(utilizator);
        }

        // POST: Admin/Utilizator/id
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Utilizator(String id, String[] listaRoluri)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guid UtilizatorID;
            if (!Guid.TryParse(id, out UtilizatorID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var utilizator = URM.getUser(UtilizatorID);
            if (utilizator == null)
            {
                return HttpNotFound();
            }
            List<ItemObject> lr = new List<ItemObject>();
            lr = URM.getRoluri();

            try
            {
                bool avemActualizare = false;
                foreach (var r in lr)
                {
                    if (listaRoluri.Contains(r.Item))
                    {
                        if (URM.updateUR(UtilizatorID, r.Item))
                        {
                            avemActualizare = true;
                        }
                    }
                    else
                    {
                        if (URM.updateUR(UtilizatorID, r.Item, false))
                        {
                            avemActualizare = true;
                        }
                    }
                }
                Session["updateUser"] = "1";
                if (avemActualizare) Session["updateUser0"] = "1";
                return RedirectToAction("Utilizator", new { id = id });
            }
            catch (Exception)
            {

                throw;
            }

            var lra = URM.getRoluri(UtilizatorID);
            foreach (var rol in lr)
            {
                if (lra.Where(x => x.ItemID == rol.ItemID).ToList().Count > 0)
                {
                    rol.SelectedItem = true;
                }
                else
                {
                    rol.SelectedItem = false;
                }
            }
            utilizator.list = lr;
            return View(utilizator);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
