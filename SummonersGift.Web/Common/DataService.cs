using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonersGift.Web
{
    public class DataService
    {
        public static SummonersGift.Data.RiotData.DataFetcher DataFetcher = null;

        public static SummonersGift.Data.Champions Champions = null;
        public static SummonersGift.Data.Summoners Summoners = null;
        public static SummonersGift.Data.RoleAndLane RoleAndLane = null;
        public static IEnumerable<string> Regions = null;
    }
}