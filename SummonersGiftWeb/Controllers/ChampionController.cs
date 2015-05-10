using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummonersGiftWeb.Controllers
{
    public class ChampionController : Controller
    {
        private SgdbContext db = new SgdbContext();
        // GET: Champion
        public ActionResult Index(int id)
        {
            var champEntry = db.Champions.Find(id);
            var name = "None";
            if (!(champEntry==null))
            {
                name = champEntry.Name;
            }
            ViewBag.ChampName = name;
            return View();
        }
    }
}