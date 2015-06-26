using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonersGift.Web.Common
{
    public class OtherStatic
    {
        public static string GoogleAnalyticsId = null;

        public string Pluralise(int count, string singular)
        {
            return count == 1 ? singular : singular + "s";
        }

        public string Pluralise(int count, string singular, string plural)
        {
             return count == 1 ? singular : plural;
        }
    }
}