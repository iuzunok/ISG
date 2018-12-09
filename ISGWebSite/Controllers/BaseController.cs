using Argem.DataServices;
using ISGWebSite.AppCode;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    [OutputCache(Duration = 0)]
    [AttributeRolKontrol(Roles = "LoginOlan")]
    // [HandleError()]
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            int HataKey = 0;
            try
            {
                using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                {
                    HataKey = oData.VeriKaydetHataYaz(ArgemSession.OpKullaniciKey, "BaseController", filterContext.Exception.Message, "", "", "", filterContext.Exception.StackTrace, "");
                }
            }
            catch (Exception ex)
            {
                Log4Net.Error("Hata VT: " + filterContext.RouteData.Values["controller"] + " " + filterContext.RouteData.Values["action"], ex);
            }

            bool isAjax = AjaxRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request);
            isAjax = (filterContext.HttpContext.Request["X-Requested-With"] == "XMLHttpRequest") || ((filterContext.HttpContext.Request.Headers != null) && (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"));
            isAjax = filterContext.HttpContext.Request.ContentType == "application/json;charset=utf-8";

            // if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Exception != null)
            if (isAjax)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Durum = "E",
                        Aciklama = "Hata Kodu :" + HataKey + " (" + filterContext.Exception.Message + ")",
                    }
                };
            }
            else
            {
                filterContext.Controller.TempData["HataKey"] = HataKey;

                //Redirect or return a view, but not both.
                // filterContext.Result = RedirectToAction("HataKontrol", "Login", new { area = "Yetki" });
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Yetki/Login/HataKontrol.cshtml"
                };
            }
        }

        //protected override void OnResultExecuting(ResultExecutingContext filterContext)
        //{

        //}

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //}

        //protected override void OnResultExecuted(ResultExecutedContext filterContext)
        //{

        //}

    }
}