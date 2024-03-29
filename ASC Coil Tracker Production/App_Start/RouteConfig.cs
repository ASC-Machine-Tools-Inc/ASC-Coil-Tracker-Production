﻿using System.Web.Mvc;
using System.Web.Routing;

namespace ASC_Coil_Tracker_Production
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var defaultRoute = new { controller = "Home", action = "Login", id = UrlParameter.Optional };

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: defaultRoute
            );
        }
    }
}