using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.FSharp.Collections;
using System.Diagnostics;


namespace SummonersGift.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                var apiKey = System.Configuration.ConfigurationManager.ConnectionStrings["devApiKey"].ConnectionString;
                Trace.TraceError("Api key pulled: " + apiKey);
                var keyList = new List<SummonersGift.Data.Utils.ApiKey>();
                keyList.Add(new Data.Utils.ApiKey(apiKey, 0.83, ""));
                DataService.DataFetcher = new Data.RiotData.DataFetcher(keyList);
            }
            catch (Exception)
            {
                var message = "Cannot get Api key from config";
                Trace.TraceError(message);
                throw new  Exception(message);
            }

            var stage = "";

            try 
            {
                stage = "champions";
                DataService.Champions = new Data.Champions("5.9.1");
                
                stage = "summoners";
                DataService.Summoners = new Data.Summoners("5.9.1");

                stage = "role and lane";
                DataService.RoleAndLane = new Data.RoleAndLane();

            }
            catch (Exception)
            {
                var message = "Static data loading failed at " + stage;
                Trace.TraceError(message);
                throw new Exception(message);
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            System.Diagnostics.Trace.TraceError(Server.GetLastError().ToString());
        }
    }
}
