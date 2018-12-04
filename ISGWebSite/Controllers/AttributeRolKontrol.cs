using System.Web;
using System.Web.Mvc;

namespace ISGWebSite.Controllers
{
    public class AttributeRolKontrol : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Kullanıcı giriş yapmamışsa login sayfasına at
            if (httpContext.Session == null)
            {
                httpContext.Response.Redirect("~/Yetki/Login/Login");
                return false;
            }
            else if (httpContext.Session["OpKullaniciKey"] == null)
            {
                httpContext.Response.Redirect("~/Yetki/Login/Login");
                return false;
            }
            else if (httpContext.Session["OpKullaniciKey"].ToString() == "")
            {
                httpContext.Response.Redirect("~/Yetki/Login/Login");
                return false;
            }
            else if (httpContext.Session["OpKullaniciKey"].ToString() == "0")
            {
                httpContext.Response.Redirect("~/Yetki/Login/Login");
                return false;
            }
            else
                return true;

            /*if (!HttpContext.Current.Request.IsAuthenticated)
            {
                httpContext.Response.Redirect("~/admin/login");
            }
            else
            {
                //cookie'deki kullanıcı idsini alıyorum
                int rolid = Convert.ToInt32(httpContext.User.Identity.Name);
                //idsini aldığım kullanıcıyı db'den çekiyorum
                var user = repo.GetQueryable().Where(c => c.ID == rolid).FirstOrDefault();
                var roles = Roles.Split(',');
                //kullanıcı admin ise 
                if (user.IsAdmin)
                {
                    if (roles.Contains("Admin"))
                        return true;
                }
                //kullanıcı company ise
                else if (user.IsCompany)
                {
                    if (roles.Contains("Company"))
                        return true;
                }
            }
            return base.AuthorizeCore(httpContext);*/
        }
    }

}