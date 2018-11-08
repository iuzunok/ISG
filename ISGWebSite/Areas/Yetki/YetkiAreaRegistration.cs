using System.Web.Mvc;

namespace ISGWebSite.Areas.Yetki
{
    public class YetkiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Yetki";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Yetki_default",
                "Yetki/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}