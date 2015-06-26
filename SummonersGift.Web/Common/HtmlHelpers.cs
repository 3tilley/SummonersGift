using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SummonersGift.Web.Common
{
    public class HtmlHelpers
    {
        public static HtmlString Pluralise(int count, string singular)
        {
            return new HtmlString(count == 1 ? singular : singular + "s");
        }

        public HtmlString Pluralise(int count, string singular, string plural)
        {
             return new HtmlString(count == 1 ? singular : plural);
        }
    }
}