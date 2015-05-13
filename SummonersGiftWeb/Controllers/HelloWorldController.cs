using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummonersGift.Web.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/
        public string Index()
        {
            return "<h1>Hello Lumphead...</h1>";
        }

        //
        // GET: /HelloWorld/Welcome/

        public ActionResult Welcome(string name, int numTimes = 1)
        {

            ViewBag.Name = SummonersGift.Web.MvcApplication.DataFetcher.GetSummonerId("EUW", name);
            ViewBag.NumTimes = numTimes;
            return View();
        }
    }
}