using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

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

        public async Task<ActionResult> Welcome(string id)
        {
            return View((await SummonersGift.Web.MvcApplication.DataFetcher.GetSummonerId("EUW", id)).Result);
        }
    }
}