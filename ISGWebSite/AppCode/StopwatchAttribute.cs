using Argem.DataServices;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace ISGWebSite.AppCode
{
    public class StopwatchAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch _stopwatch;

        public StopwatchAttribute()
        {
            _stopwatch = new Stopwatch();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopwatch.Restart();
            filterContext.Controller.TempData["ServerCalismaSure"] = _stopwatch;
            // _stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopwatch.Stop();

            try
            {
                using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                {
                    oData.SayfaZiyaretYaz(ArgemSession.OpKullaniciKey, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString(), _stopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Log4Net.Error("Hata Ziyeret VT: " + filterContext.RouteData.Values["controller"] + " " + filterContext.RouteData.Values["action"] + " " + _stopwatch.ElapsedMilliseconds.ToString(), ex);
            }

            //    var httpContext = filterContext.HttpContext;
            //    var response = httpContext.Response;
            //    var elapsed = _stopwatch.Elapsed.ToString();

            //    // Works for Cassini and IIS
            //    //response.Write(string.Format("<!-- X-Stopwatch: {0} -->", elapsed));  

            //    // Works for IIS
            //    // filterContext.Controller.TempData["ServerCalismaSure"] = elapsed;
            //    response.AddHeader("X-Stopwatch", elapsed);
        }
    }

}