using Argem.DataServices;
using ArgemUtil;
using ISGWebSite.AppCode;
using ISGWebSite.Controllers;
using ISGWebSite.Models;
using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Data;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // CaptchaTextBox için b bak
        // https://github.com/managedfusion/managedfusion-web/blob/master/src/Web/Mvc/Extensions/HtmlHelper.cs

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SistemeGirisYap(KullaniciLoginModel oKullaniciLoginModel)
        {
            SonucModel<KullaniciLoginModel> oSonucModel = new SonucModel<KullaniciLoginModel>() { Durum = "H", Aciklama = "" };

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(oKullaniciLoginModel.KullaniciAd) || string.IsNullOrEmpty(oKullaniciLoginModel.Parola))
                    oSonucModel.Aciklama = "Kullanıcı adı ve/veya şifre boş";
                else
                {
                    ArgemSQL oSQL = new ArgemSQL();
                    oSQL.CommandText =
                        "select * " +
                        "from   public.\"KULLANICI\" ";
                    oSQL.Esit("KullaniciAd", oKullaniciLoginModel.KullaniciAd, KolonTipi.String);
                    oSQL.Esit("Parola", oKullaniciLoginModel.Parola, KolonTipi.String);
                    using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                    {
                        DataTable dt = new DataTable();
                        oData.DataGetir(ref dt, oSQL);

                        if (dt.Rows.Count > 0)
                        {
                            // todo log kaydı yap

                            if (dt.Rows[0]["AktifPasifTipNo"].ToString() == "100")
                            {
                                ArgemSession.OpKullaniciKey = Convert.ToInt32(dt.Rows[0]["KullaniciKey"]);
                                ArgemSession.OpKullaniciAd = dt.Rows[0]["Ad"].ToString();
                                ArgemSession.OpKullaniciSoyad = dt.Rows[0]["Soyad"].ToString();
                                oSonucModel.Durum = "";
                            }
                            else
                                oSonucModel.Aciklama = "Kullanıcı pasif durumda";
                        }
                        else
                            oSonucModel.Aciklama = "Kullanıcı adı ve/veya şifre hatalı";
                    }


                    /*string sSQL =
                        "select * " +
                        "from   public.\"KULLANICI\" " +
                        "where  \"KullaniciAd\" = '" + oKullaniciLoginModel.KullaniciAd + "' " +
                        "       and \"Parola\" = '" + oKullaniciLoginModel.Parola + "'";
                    DataTable dtP = DBUtil.VeriGetirDT(sSQL);
                    if (dtP != null)
                    {
                        if (dtP.Rows.Count > 0)
                        {
                            if (dtP.Rows[0]["AktifPasifTipNo"].ToString() == "100")
                            {
                                Session["OpKullaniciKey"] = dtP.Rows[0]["KullaniciKey"].ToString();
                                Session["OpKullaniciAd"] = dtP.Rows[0]["Ad"].ToString();
                                Session["OpKullaniciSoyad"] = dtP.Rows[0]["Soyad"].ToString();
                                oSonucModel.Durum = "";
                            }
                            else
                                oSonucModel.Aciklama = "Kullanıcı pasif durumda";
                        }
                        else
                            oSonucModel.Aciklama = "Kullanıcı adı ve/veya şifre hatalı";
                    }
                    else
                        oSonucModel.Aciklama = "Kullanıcı adı ve/veya şifre hatalı.";*/
                }
            }
            else
                oSonucModel.Aciklama = "Gerekli alanları giriniz";

            return Json(oSonucModel, JsonRequestBehavior.DenyGet);
        }

        public ActionResult LogOff()
        {
            // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            return RedirectToAction("Login", "Login", "Yetki");
        }

        public ActionResult AnaSayfa()
        {
            return View();
        }

        public ActionResult HataKontrol()
        {
            return View();
        }
    }
}