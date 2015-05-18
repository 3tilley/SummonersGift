using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SummonersGift.Web.Controllers
{
    public class SummonerController : Controller
    {
        // GET: Summoner
        public ActionResult Index()
        {
            return View("Index");
        }

        // GET: Summoner?region=...&name=...
        public async Task<ActionResult> SummonerByName(string region, string name)
        {
            return View("SummonerByName",
                    await DataService.DataFetcher.AsyncGetSummonerIdAndMatchesThisSeason(region, name));
        }
    }
}