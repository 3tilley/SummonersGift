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
        public static SummonersGift.Data.DataFetcher DataFetcher = null;

        protected void Application_Start()
        {
            var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["devApiKey"].ConnectionString;
            var keyList = new List<SummonersGift.Data.Utils.ApiKey>();
            keyList.Add(new Data.Utils.ApiKey(apiKey, 0.83, ""));

            DataFetcher = new SummonersGift.Data.DataFetcher(keyList);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
