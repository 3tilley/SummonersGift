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
        readonly string cookieName = "user";
        readonly string defaultValueName = "defaultRegion";
        // GET: public
        public ActionResult SummonerLookup()
        {
            ViewBag.Title = "Lookup Summoner";

            string defaultRegion = "euw";
            HttpCookie defaultValueCookie = Request.Cookies[cookieName];

            defaultRegion = (defaultValueCookie == null) ? "euw" : defaultValueCookie[defaultValueName];

            //if (defaultRegionCookie != null)
            //{
            //    defaultRegion = defaultRegionCookie.Value;
            //}

            ViewBag.DefaultValue = defaultRegion;

            var summonerSearch = new SummonersGift.Models.View.SummonerSearch();
            summonerSearch.region = defaultRegion;
            summonerSearch.makeDefaultRegion = false;
            return View("Index", summonerSearch);
        }

        // GET: Summoner/region/name
        public async Task<ActionResult> SummonerByName(string region, string name, bool makeDefaultRegion = false)
        {
            if ((region==null) | (name==null))
            {
                return SummonerLookup();
            }
            ViewBag.Title = name;

            if (makeDefaultRegion)//.HasValue & makeDefaultRegion.Value)
            {
                HttpCookie defaultValueCookie = Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
                defaultValueCookie.Values.Add(defaultValueName, region);
                defaultValueCookie.Expires.AddYears(5);
                Response.Cookies.Add(defaultValueCookie);
            }

            Trace.TraceInformation("Summoner lookup: " + region + " : " + name);

            return View("SummonerByName",
                    await DataService.DataFetcher.GetSummonerLeagueAndMatchesThisSeasonAsync(region, name));
        }
    }
}