using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.FSharp.Collections;
using System.Diagnostics;
using System.Configuration;

namespace SummonersGift.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                var apiKeyObj = ConfigurationManager.ConnectionStrings["devApiKey"];
                var apiKey = "";
                if (apiKeyObj == null)
                {
                    apiKey = System.Configuration.ConfigurationManager.AppSettings["devApiKey"];
                }
                else
                {
                    apiKey = apiKeyObj.ConnectionString;
                }
                Trace.TraceInformation("Api key pulled: " + apiKey);
                var keyList = new List<SummonersGift.Data.Utils.ApiKey>();
                keyList.Add(new Data.Utils.ApiKey(apiKey, 500.0 / 600.0, ""));
                
                //var pool = new Data.RiotRequestPool.BasicPool(500.0 / 600.0);
                var pool = new Data.RiotRequestPool.MailboxPool(10);

                DataService.DataFetcher = new Data.RiotData.DataFetcher(keyList, pool);
            }
            catch (Exception)
            {
                var message = "Cannot get Api key from config";
                Trace.TraceError(message);
                throw new  Exception(message);
            }

            try
            {
                var googleId = ConfigurationManager.AppSettings["GoogleAnalyticsId"];
                Common.OtherStatic.GoogleAnalyticsId = googleId;
                Trace.TraceInformation("Google Analytics Id pull: " + googleId);

            }
            catch (Exception)
            {
                var message = "Unable to get Google Analytics Id from config";
                Trace.TraceError(message);
                throw new Exception(message);
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

                stage = "region";
                DataService.Regions = new Data.Region().Regions;

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
