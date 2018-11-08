using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    [HandleError()]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //ViewResult view = new ViewResult();
            //view.ViewName = "Error";
            //filterContext.Result = view;
            //filterContext.ExceptionHandled = true;
        }

       
    }
}