using ISGWebSite.Controllers;
using ISGWebSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Data;
using System.Web;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class AccountController : BaseController
    {
        #region login kontrol

        public ActionResult Unauthorised()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

     
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginXX(LoginViewModel oLoginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(oLoginViewModel.KullaniciAd) || string.IsNullOrEmpty(oLoginViewModel.Parola))
                {
                    ModelState.AddModelError("", "Kullanıcı adı ve/veya şifre boş");
                    return View(oLoginViewModel);
                }
                else
                {
                    string sSQL =
                        "select * " +
                        "from   public.\"KULLANICI\" " +
                        "where  \"KullaniciAd\" = '" + oLoginViewModel.KullaniciAd + "' " +
                        "       and \"Parola\" = '" + oLoginViewModel.Parola + "'";
                    DataTable dt = DBUtil.VeriGetirDT(sSQL);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["AktifPasifTipNo"].ToString() == "100")
                            {
                                Session["OpKullaniciKey"] = dt.Rows[0]["KullaniciKey"].ToString();
                                Session["OpKullaniciAd"] = dt.Rows[0]["Ad"].ToString();
                                Session["OpKullaniciSoyad"] = dt.Rows[0]["Soyad"].ToString();

                                /*if (Url.IsLocalUrl(returnUrl))
                                    return Redirect(returnUrl);
                                else*/
                                return RedirectToAction("AnaSayfa", "Account", new { area = "" });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Kullanıcı pasif durumda");
                                return View(oLoginViewModel);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Kullanıcı adı ve/veya şifre hatalı");
                            return View(oLoginViewModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı ve/veya şifre hatalı.");
                        return View(oLoginViewModel);
                    }
                }

                //// CustomMembership oCustomMembership = new CustomMembership();
                //DataTable dt = oCustomMembership.ValidateUser(loginView.KullaniciAd, loginView.Sifre, out Sonuc);
                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (dt.Rows[0]["AktifPasifTipNo"].ToString() == "1")
                //        {
                //            /*List<Role> oRoles = new List<Role>();

                //            Role oRole = new Role()
                //            {
                //                RoleId = 1,
                //                RoleName = "Admin"
                //            };
                //            oRoles.Add(oRole);*/

                //            KullaniciModel oUser = new KullaniciModel()
                //            {
                //                KullaniciKey = 1,
                //                // Email = loginView.KullaniciAd,
                //                KullaniciAd = loginView.KullaniciAd,
                //                Ad = dt.Rows[0]["Ad"].ToString(),
                //                Soyad = dt.Rows[0]["Soyad"].ToString(),
                //                // ActivationCode = new Guid(),
                //                // IsActive = true,
                //                // Password = "",
                //                // Roles = oRoles
                //            };
                //            var user = new CustomMembershipUser(oUser);

                //            // var user = (CustomMembershipUser)oCustomMembership.GetUser(loginView.UserName, false);
                //            // if (user != null)
                //            // {
                //            CustomSerializeModel oCustomSerializeModel = new CustomSerializeModel()
                //            {
                //                UserId = user.UserId,
                //                FirstName = user.FirstName,
                //                LastName = user.LastName,
                //                RoleName = new List<string>()  // user.Roles.Select(r => r.RoleName).ToList()
                //            };

                //            var ident = new ClaimsIdentity(new[] {
                //            // adding following 2 claim just for supporting default antiforgery provider
                //            new Claim(ClaimTypes.NameIdentifier, user.UserName),
                //            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                //            new Claim(ClaimTypes.Name, user.UserName),
                //            // optionally you could add roles if any
                //            new Claim(ClaimTypes.Role, "RoleName"),
                //            new Claim(ClaimTypes.Role, "AnotherRole")},
                //                DefaultAuthenticationTypes.ApplicationCookie);
                //            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);

                //            string userData = JsonConvert.SerializeObject(oCustomSerializeModel);
                //            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, loginView.KullaniciAd, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

                //            string enTicket = FormsAuthentication.Encrypt(authTicket);
                //            HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                //            Response.Cookies.Add(faCookie);
                //            // }

                //            if (Url.IsLocalUrl(returnUrl))
                //                return Redirect(returnUrl);
                //            else
                //                return RedirectToAction("AnaSayfa", "Account", new { area = "" });
                //        }
                //        else
                //        {
                //            ModelState.AddModelError("", "Kullanıcı pasif durumda");
                //            return View(loginView);
                //        }
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", "Kullanıcı adı ve/veya şifre hatalı");
                //        return View(loginView);
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", Sonuc);
                //    return View(loginView);
                //}
            }
            ModelState.AddModelError("", "Tanımsız hata. Kullanıcı yetki kontrol hatası");
            return View(oLoginViewModel);
        }


        ////
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //// public async Task<ActionResult> Login(LoginViewModel loginView, string returnUrl)
        //public ActionResult LoginXXX(LoginViewModel loginView, string returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string Sonuc = "";
        //        CustomMembership oCustomMembership = new CustomMembership();
        //        DataTable dt = oCustomMembership.ValidateUser(loginView.KullaniciAd, loginView.Sifre, out Sonuc);
        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Rows[0]["AktifPasifTipNo"].ToString() == "1")
        //                {
        //                    /*List<Role> oRoles = new List<Role>();

        //                    Role oRole = new Role()
        //                    {
        //                        RoleId = 1,
        //                        RoleName = "Admin"
        //                    };
        //                    oRoles.Add(oRole);*/

        //                    KullaniciModel oUser = new KullaniciModel()
        //                    {
        //                        KullaniciKey = 1,
        //                        // Email = loginView.KullaniciAd,
        //                        KullaniciAd = loginView.KullaniciAd,
        //                        Ad = dt.Rows[0]["Ad"].ToString(),
        //                        Soyad = dt.Rows[0]["Soyad"].ToString(),
        //                        // ActivationCode = new Guid(),
        //                        // IsActive = true,
        //                        // Password = "",
        //                        // Roles = oRoles
        //                    };
        //                    var user = new CustomMembershipUser(oUser);

        //                    // var user = (CustomMembershipUser)oCustomMembership.GetUser(loginView.UserName, false);
        //                    // if (user != null)
        //                    // {
        //                    CustomSerializeModel oCustomSerializeModel = new CustomSerializeModel()
        //                    {
        //                        UserId = user.UserId,
        //                        FirstName = user.FirstName,
        //                        LastName = user.LastName,
        //                        RoleName = new List<string>()  // user.Roles.Select(r => r.RoleName).ToList()
        //                    };

        //                    var ident = new ClaimsIdentity(new[] {
        //                    // adding following 2 claim just for supporting default antiforgery provider
        //                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
        //                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
        //                    new Claim(ClaimTypes.Name, user.UserName),
        //                    // optionally you could add roles if any
        //                    new Claim(ClaimTypes.Role, "RoleName"),
        //                    new Claim(ClaimTypes.Role, "AnotherRole")},
        //                        DefaultAuthenticationTypes.ApplicationCookie);
        //                    HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);

        //                    string userData = JsonConvert.SerializeObject(oCustomSerializeModel);
        //                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, loginView.KullaniciAd, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

        //                    string enTicket = FormsAuthentication.Encrypt(authTicket);
        //                    HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
        //                    Response.Cookies.Add(faCookie);
        //                    // }

        //                    if (Url.IsLocalUrl(returnUrl))
        //                        return Redirect(returnUrl);
        //                    else
        //                        return RedirectToAction("AnaSayfa", "Account", new { area = "" });
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Kullanıcı pasif durumda");
        //                    return View(loginView);
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Kullanıcı adı ve/veya şifre hatalı");
        //                return View(loginView);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", Sonuc);
        //            return View(loginView);
        //        }
        //    }
        //    ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
        //    return View(loginView);
        //}

        public ActionResult LogOff()
        {
            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion

        public ActionResult SayfaBulunamadi(string aspxerrorpath)
        {
            if (string.IsNullOrEmpty(aspxerrorpath))
                ViewBag.Message = "Tanımsız sayfa kodu";
            else
            {
                ViewBag.ReturnUrl = aspxerrorpath;
                ViewBag.Message = "Sayfa bulunamadı. " + aspxerrorpath;
            }
            return View();
        }

        public ActionResult HataKontrol()
        {
            return View();
        }

        public ActionResult AnaSayfa()
        {
            return View();
        }


        private string UzunAdBul(int LookNo)
        {
            if (LookNo == 1)
                return "Personel";
            else if (LookNo == 2)
                return "Dış personel";
            return "";
        }

    }
}