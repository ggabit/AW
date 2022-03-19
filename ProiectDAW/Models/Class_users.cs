using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProiectDAW.Models;

namespace ProiectDAW
{
    namespace Models
    {
        public class ItemObject
        {
            public string ItemID { get; set; }
            public string Item { get; set; }
            public bool SelectedItem { get; set; }
            public List<ItemObject> list { get; set; }
        }
        public static class URM
        {
            public static List<ItemObject> getRoluri()
            {
                var rm = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                var roluri = (from r in rm.Roles
                              orderby r.Name
                              select new ItemObject() { ItemID = r.Id, Item = r.Name }).ToList();

                return roluri;
            }
            public static List<ItemObject> getUseri()
            {
                var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var useri = (from u in um.Users
                             orderby u.UserName
                             select new ItemObject() { ItemID = u.Id, Item = u.UserName }).ToList();
                return useri;
            }

            public static ItemObject getRol(Guid ItemID)
            {
                var rm = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                var rol = (from r in rm.Roles
                           where r.Id == ItemID.ToString()
                           select new ItemObject() { ItemID = r.Id, Item = r.Name }).FirstOrDefault();

                return rol;
            }
            public static List<ItemObject> getUseri(Guid RolID)
            {
                var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var useri = (from u in um.Users
                             where u.Roles.Where(t => t.RoleId == RolID.ToString()).Count() > 0
                             orderby u.UserName
                             select new ItemObject() { ItemID = u.Id, Item = u.UserName }).ToList();
                return useri;
            }

            public static ItemObject getUser(Guid ItemID)
            {
                var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = (from u in um.Users
                            where u.Id == ItemID.ToString()
                            select new ItemObject() { ItemID = u.PhoneNumber, Item = u.UserName }).FirstOrDefault();
                return user;
            }

            public static bool isUserInRol(Guid UserId, Guid RolId)
            {
                var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = um.Users.Where(x => x.Id == UserId.ToString()).FirstOrDefault();
                var rm= HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                var rol = um.Users.Where(x => x.Id == RolId.ToString()).FirstOrDefault();
                if (user != null)
                {
                    //if (user.Roles.Contains(rol))
                    //{

                    //}
                }
                return false;
            }

            public static List<ItemObject> getRoluri(Guid UserID)
            {
                var rm = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                var roluri = (from r in rm.Roles
                              where r.Users.Where(t => t.UserId == UserID.ToString()).Count() > 0
                              orderby r.Name
                              select new ItemObject() { ItemID = r.Id, Item = r.Name }).ToList();

                return roluri;
            }

            public static bool updateUR(Guid UserID, string Rol, bool esteBifat = true)
            {
                var um = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                bool esteActualizare = false;
                if (esteBifat)
                {
                    if (!um.IsInRole(UserID.ToString(), Rol))
                    {
                        um.AddToRole(UserID.ToString(), Rol);
                        esteActualizare = true;
                    }
                }
                else
                {
                    if (um.IsInRole(UserID.ToString(), Rol))
                    {
                        um.RemoveFromRole(UserID.ToString(), Rol);
                        esteActualizare = true;
                    }
                }
                return esteActualizare;
            }
        }
    }
}