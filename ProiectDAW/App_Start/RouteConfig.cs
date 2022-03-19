using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProiectDAW
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "judete",
                url: "Judete",
                defaults: new { controller = "Judet", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "persoane",
                url: "Persoane",
                defaults: new { controller = "Persoana", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "caini",
                url: "Caini",
                defaults: new { controller = "Caine", action = "Paginare", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "adoptii",
                url: "Adoptii",
                defaults: new { controller = "Adoptie", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
