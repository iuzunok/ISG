using ISGWebSite.AppCode;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ISGWebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new StopwatchAttribute());
           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // ClientDataTypeModelValidatorProvider.ResourceClassKey = "Messages";
            // DefaultModelBinder.ResourceClassKey = "Messages";

            //set the antiforgery claim to user id
            // AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            log4net.Config.XmlConfigurator.Configure();
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    HttpContext httpContext = HttpContext.Current;
        //    if (httpContext != null)
        //    {
        //        /*// if (httpContext.CurrentHandler is System.Web.HttpNotFoundHandler)

        //        RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
        //        if (requestContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            httpContext.Response.Clear();
        //            string controllerName = requestContext.RouteData.GetRequiredString("controller");
        //            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
        //            IController controller = factory.CreateController(requestContext, controllerName);
        //            ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

        //            JsonResult jsonResult = new JsonResult
        //            {
        //                Data = new { success = false, serverError = "500" },
        //                JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //            };
        //            jsonResult.ExecuteResult(controllerContext);
        //            httpContext.Response.End();
        //        }
        //        else
        //        {*/
        //            httpContext.Response.Redirect("~/Views/Shared/Error.cshtml");
        //        //}
        //    }
        //}

    }
}
