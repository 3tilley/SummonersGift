using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.FSharp.Collections;

namespace SummonersGift.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["devApiKey"].ConnectionString;
            var keyList = new List<SummonersGift.Data.Utils.ApiKey>();
            keyList.Add(new Data.Utils.ApiKey(apiKey, 0.83, ""));

            DataService.DataFetcher = new Data.RiotData.DataFetcher(keyList);
            DataService.Champions = new Data.Champions("5.9.1");
            DataService.Summoners = new Data.Summoners("5.9.1");

            DataService.RoleAndLane = new Data.RoleAndLane();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
