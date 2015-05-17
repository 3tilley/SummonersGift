using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SummonersGift.Models.Entity;

namespace SummonersGift.Web.Controllers
{
    public class ChampionController : Controller
    {
        private SgdbContext db = new SgdbContext();
        // GET: Champion
        public ActionResult Index(int id)
        {
            var name = "hello"; // Data.Champions.ChampNameOrDefault(id, "None");

            ViewBag.ChampName = name;
            return View();
        }
    }
}