using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    [OutputCache(Duration = 0)]
    [AttributeRolKontrol(Roles = "LoginOlan")]
    [HandleError()]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            /*ViewResult view = new ViewResult();
            view.ViewName = "~/Account/HataKontrol";
            filterContext.Result = view;
            filterContext.ExceptionHandled = true;*/
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }

    }
}