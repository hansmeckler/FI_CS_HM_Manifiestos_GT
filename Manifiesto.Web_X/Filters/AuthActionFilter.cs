using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manifiesto.Web.Filters
{
  public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            int tipo_acceso = Convert.ToInt32(ctx.Session["tipo_acceso"]);

            if (tipo_acceso == null)
            {
                // check if a new session id was generated
                filterContext.Result = new RedirectResult("~/Users/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}