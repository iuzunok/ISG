using ISGWebSite.Controllers;
using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class KullaniciController : BaseController
    {
        // GET: Yetki/Kullanici
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KullaniciKayit(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();
                return View(aryKullaniciModel);
            }
            else
                return HttpNotFound("1111");
        }

        public ActionResult KullaniciKayit()
        {
            List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();
            return View(aryKullaniciModel);
        }

        public ActionResult KullaniciAra(FormCollection formCollection)
        {
            // try
            // {
            // throw new Exception("Test Exception");

            if (ModelState.IsValid)
            {
                // string s = "controller seviye hata";
                // int i = Convert.ToInt16(s);

                List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();
                // DataTable dtPersonelAra = null;
                // DataSet ds = null;

                if (formCollection.Count > 0)
                {
                    string KullaniciAd = formCollection["txtKullaniciAd"];
                    string Ad = formCollection["txtAd"];
                    string Soyad = formCollection["txtSoyad"];

                    string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                    if (KullaniciAd.Trim() != "")
                        sSQL += " where \"KullaniciAd\" like '%" + KullaniciAd.Trim() + "%'";
                    if (Ad.Trim() != "")
                        sSQL += " where \"Ad\" like '%" + Ad.Trim() + "%'";
                    if (Soyad.Trim() != "")
                        sSQL += " where \"Soyad\" like '%" + Soyad.Trim() + "%'";
                    DataSet ds = DBUtil.VeriGetirDS(sSQL);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                        KullaniciModel oKullaniciModel = new KullaniciModel()
                        {
                            KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                            KullaniciAd = dr["KullaniciAd"].ToString(),
                            Ad = dr["Ad"].ToString(),
                            Soyad = dr["Soyad"].ToString(),
                            KullaniciTipNo = KullaniciTipNo,
                            KullaniciTipNoUzunAd = UzunAdBul(KullaniciTipNo)
                            // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                            // UTar = Convert.ToDateTime(dr["UTar"])
                        };
                        aryKullaniciModel.Add(oKullaniciModel);
                    }

                    /*oKullaniciModel = new KullaniciModel() { KullaniciKey = 2, KullaniciAd = "2", Ad = "iso", Soyad = "sak", KullaniciTipNo = 1, UKullaniciKey = 1, UTar = DateTime.Now };
                    aryKullaniciModel.Add(oKullaniciModel);*/
                }
                // else
                // TempData["Sonuc"] = "Kayıt bulunamadı";

                return View(aryKullaniciModel);
                // return View(ds);
            }
            return View("sdsdfds");
            // }
            // catch (Exception ex)
            // {
            // throw ex;
            // }
        }


        //#region login kontrol

        //public ActionResult Unauthorised()
        //{
        //    return View();
        //}
        ////
        //// GET: /Account/Login
        //[AllowAnonymous]
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}

        ////
        //// POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //// public async Task<ActionResult> Login(LoginViewModel loginView, string returnUrl)
        //public ActionResult Login(LoginViewModel loginView, string returnUrl)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string Sonuc = "";
        //        CustomMembership oCustomMembership = new CustomMembership();
        //        DataTable dt = oCustomMembership.ValidateUser(loginView.UserName, loginView.Password, out Sonuc);
        //        if (dt != null)
        //        {
        //            if (dt.Rows.Count > 0)
        //            {
        //                if (dt.Rows[0]["AktifPasifTipNo"].ToString() == "1")
        //                {
        //                    List<Role> oRoles = new List<Role>();

        //                    Role oRole = new Role()
        //                    {
        //                        RoleId = 1,
        //                        RoleName = "Admin"
        //                    };
        //                    oRoles.Add(oRole);

        //                    User oUser = new User()
        //                    {
        //                        UserId = 1,
        //                        Email = loginView.UserName,
        //                        Username = loginView.UserName,
        //                        FirstName = dt.Rows[0]["Ad"].ToString(),
        //                        LastName = dt.Rows[0]["Soyad"].ToString(),
        //                        ActivationCode = new Guid(),
        //                        IsActive = true,
        //                        Password = "",
        //                        Roles = oRoles
        //                    };
        //                    var user = new CustomMembershipUser(oUser);
                                                       
        //                    // var user = (CustomMembershipUser)oCustomMembership.GetUser(loginView.UserName, false);
        //                    // if (user != null)
        //                    // {
        //                    CustomSerializeModel userModel = new CustomSerializeModel()
        //                    {
        //                        UserId = user.UserId,
        //                        FirstName = user.FirstName,
        //                        LastName = user.LastName,
        //                        RoleName = user.Roles.Select(r => r.RoleName).ToList()
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

        //                    string userData = JsonConvert.SerializeObject(userModel);
        //                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData);

        //                    string enTicket = FormsAuthentication.Encrypt(authTicket);
        //                    HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
        //                    Response.Cookies.Add(faCookie);
        //                    // }

        //                    if (Url.IsLocalUrl(returnUrl))
        //                        return Redirect(returnUrl);
        //                    else
        //                        return RedirectToAction("Index", "Home", new { area = "" });
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

        //#endregion



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