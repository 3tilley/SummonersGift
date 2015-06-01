using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SummonersGift.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Summoner",
            //    url: "Summoner",
            //    defaults: new
            //    {
            //        controller = "Summoner",
            //        action = "Index"
            //    }
            //);

            routes.MapRoute(
                name: "SummonerByName",
                url: "Summoner",
                defaults: new { controller = "Summoner", action = "SummonerByName" }
            );

            routes.MapRoute(
                name: "Summoner",
                url: "Summoner",
                defaults: new
                {
                    controller = "Summoner",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: "HelloWorld",
                url: "HelloWorld/Welcome/{id}",
                defaults: new { controller = "HelloWorld", action = "Welcome" }
            );

            routes.MapRoute(
                name: "Champion",
                url: "Champion/{id}/",
                defaults: new { controller = "Champion", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
