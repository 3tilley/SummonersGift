using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.Http.Filters;
//using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Diagnostics;

namespace SummonersGift.Web
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            Trace.TraceError(context.Exception.ToString());

            var res = new ContentResult();
            res.Content = context.Exception.ToString();
            context.Result = res;
            
        }
    }
}