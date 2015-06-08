using System.Web;
using System.Web.Mvc;

namespace SummonersGift.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
