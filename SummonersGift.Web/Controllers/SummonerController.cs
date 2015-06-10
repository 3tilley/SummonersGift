using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SummonersGift.Web.Controllers
{
    public class SummonerController : Controller
    {
        // GET: Summoner
        public ActionResult SummonerLookup()
        {
            ViewBag.Title = "Lookup Summoner";
            return View("Index");
        }

        // GET: Summoner/region/name
        public async Task<ActionResult> SummonerByName(string region, string name)
        {
            if ((region==null) | (name==null))
            {
                return SummonerLookup();
            }
            ViewBag.Title = name;

            Trace.TraceError("Summoner lookup: " + region + " : " + name);

            return View("SummonerByName",
                    await DataService.DataFetcher.GetSummonerLeagueAndMatchesThisSeasonAsync(region, name));
        }
    }
}