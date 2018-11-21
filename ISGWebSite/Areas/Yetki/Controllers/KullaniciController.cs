using ISGWebSite.AppCode;
using ISGWebSite.Controllers;
using ISGWebSite.Models;
using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    // https://www.youtube.com/watch?v=_r6laMn70FA paging
    public class KullaniciController : BaseController
    {
        public ActionResult KullaniciAra()
        {
            return View();
        }

        public ActionResult KullaniciAraTemp()
        {
            return View();
        }

        public ActionResult KullaniciAraTempTemp()
        {
            return View();
        }

        public ActionResult KullaniciAraMVC(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                // hata oluşturmak için
                // string s = "controller seviye hata";
                // int i = Convert.ToInt16(s);

                List<KullaniciModelAra> aryKullaniciModelAra = new List<KullaniciModelAra>();

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
                        int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                        KullaniciModelAra oKullaniciModelAra = new KullaniciModelAra()
                        {
                            KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                            KullaniciAd = dr["KullaniciAd"].ToString(),
                            Ad = dr["Ad"].ToString(),
                            Soyad = dr["Soyad"].ToString(),
                            KullaniciTipNo = KullaniciTipNo,

                            KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),
                            AktifPasifTipNo = AktifPasifTipNo,
                            AktifPasifTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, AktifPasifTipNo)
                        };
                        aryKullaniciModelAra.Add(oKullaniciModelAra);
                    }
                }
                // else
                // TempData["Sonuc"] = "Kayıt bulunamadı";

                return View(aryKullaniciModelAra);
            }
            return View("sdsdfds");
        }

        public JsonResult KullaniciAraSonuc(KullaniciModelAra oKullaniciModelAra)
        {
            // System.Threading.Thread.Sleep(2000);
            List<KullaniciModelAra> aryKullaniciModelAra = new List<KullaniciModelAra>();
            if (ModelState.IsValid)
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                string Where = "";
                if (oKullaniciModelAra.KullaniciKey != 0)
                    Where = " \"KullaniciKey\" = " + oKullaniciModelAra.KullaniciKey;
                else
                {
                    if (!string.IsNullOrEmpty(oKullaniciModelAra.KullaniciAd))
                        Where = " \"KullaniciAd\" like '%" + oKullaniciModelAra.KullaniciAd.Trim() + "%'";
                    if (!string.IsNullOrEmpty(oKullaniciModelAra.Ad))
                    {
                        if (Where != "")
                            Where += " and ";
                        Where += "  \"Ad\" like '%" + oKullaniciModelAra.Ad.Trim() + "%'";
                    }
                    if (!string.IsNullOrEmpty(oKullaniciModelAra.Soyad))
                    {
                        if (Where != "")
                            Where += " and ";
                        Where += " \"Soyad\" like '%" + oKullaniciModelAra.Soyad.Trim() + "%'";
                    }
                    if (oKullaniciModelAra.KullaniciTipNo != 0)
                    {
                        if (Where != "")
                            Where += " and ";
                        Where += " \"KullaniciTipNo\" = " + oKullaniciModelAra.KullaniciTipNo + "";
                    }
                    if (oKullaniciModelAra.AktifPasifTipNo != 0)
                    {
                        if (Where != "")
                            Where += " and ";
                        Where += " \"AktifPasifTipNo\" = " + oKullaniciModelAra.AktifPasifTipNo + "";
                    }
                }
                if (Where != "")
                    sSQL += "where " + Where;

                sSQL += "order by \"Ad\", \"Soyad\" ";

               DataSet ds = DBUtil.VeriGetirDS(sSQL);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                    oKullaniciModelAra = new KullaniciModelAra()
                    {
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),
                        AktifPasifTipNo = AktifPasifTipNo,
                        AktifPasifTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, AktifPasifTipNo)
                    };
                    aryKullaniciModelAra.Add(oKullaniciModelAra);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
            }

            return Json(aryKullaniciModelAra, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KullaniciKayit(string Durum, string Key)
        {
            KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit()
            {
                IslemDurum = "H",
                IslemAciklama = "Tanımsız hata"
            };

            if (string.IsNullOrEmpty(Durum) && string.IsNullOrEmpty(Key))
            {
                oKullaniciModelKayit.IslemAciklama = "Hatalı parametre";
                return PartialView(oKullaniciModelKayit);
            }
            else if (Durum == "I")
            {
                oKullaniciModelKayit = new KullaniciModelKayit()
                {
                    IslemDurum = "I",
                    KullaniciKey = 0,
                    KullaniciAd = "",
                    Ad = "",
                    Soyad = "",

                    KullaniciTipNo = 0,
                    KullaniciTipNoUzunAd = "",
                    KullaniciTipNolar = CacheHelper.LookGetir(CacheHelper.DatabaseTipNo.Yetki, "KullaniciTipNo"),

                    AktifPasifTipNo = 0,
                    AktifPasifTipNoUzunAd = "",
                    AktifPasifTipNolar = CacheHelper.LookGetir(CacheHelper.DatabaseTipNo.Yetki, "AktifPasifTipNo")

                    // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                    // UTar = Convert.ToDateTime(dr["UTar"])
                };
                return PartialView(oKullaniciModelKayit);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\" ";
                sSQL += "where \"KullaniciKey\" = " + Key;
                DataSet ds = DBUtil.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                    oKullaniciModelKayit = new KullaniciModelKayit()
                    {
                        IslemDurum = "U",
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),

                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),
                        KullaniciTipNolar = CacheHelper.LookGetir(CacheHelper.DatabaseTipNo.Yetki, "KullaniciTipNo"),

                        AktifPasifTipNo = AktifPasifTipNo,
                        AktifPasifTipNoUzunAd = "",
                        AktifPasifTipNolar = CacheHelper.LookGetir(CacheHelper.DatabaseTipNo.Yetki, "AktifPasifTipNo")

                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    return PartialView(oKullaniciModelKayit);
                }
                else
                    oKullaniciModelKayit.IslemAciklama = "Kullanıcı bilgisine ulaşılamadı";
            }

            return PartialView(oKullaniciModelKayit);
        }

        [HttpPost]
        public JsonResult KullaniciKayit(KullaniciModelKayit oKullaniciModelKayit)
        {
            int KullaniciKey = oKullaniciModelKayit.KullaniciKey;
            string KullaniciAd = oKullaniciModelKayit.KullaniciAd;
            string Ad = oKullaniciModelKayit.Ad;
            string Soyad = oKullaniciModelKayit.Soyad;
            int KullaniciTipNo = oKullaniciModelKayit.KullaniciTipNo;
            int AktifPasifTipNo = oKullaniciModelKayit.AktifPasifTipNo;

            oKullaniciModelKayit.IslemDurum = "H";
            if (string.IsNullOrEmpty(KullaniciAd))
                oKullaniciModelKayit.IslemAciklama = "Kullanıcı adı boş olamaz";
            else if (string.IsNullOrEmpty(Ad))
                oKullaniciModelKayit.IslemAciklama = "Ad boş olamaz";
            else if (string.IsNullOrEmpty(Soyad))
                oKullaniciModelKayit.IslemAciklama = "Soyad boş olamaz";
            else
            {
                if (KullaniciKey == 0)
                {
                    string sSQL =
                        "insert into public.\"KULLANICI\" " +
                        "       (\"KullaniciAd\", \"Ad\", \"Soyad\", \"KullaniciTipNo\", \"AktifPasifTipNo\", \"Parola\", \"UKullaniciKey\", \"UTar\") " +
                        "values ('" + KullaniciAd + "','" + Ad + "','" + Soyad + "', " + KullaniciTipNo + ", " + AktifPasifTipNo + ", '123', 1, CURRENT_DATE) " +
                        "returning \"KullaniciKey\" ";
                    string SonucKullaniciKey = DBUtil.SorguCalistir(sSQL);
                    if (SonucKullaniciKey != "0")
                        oKullaniciModelKayit.IslemDurum = "OK";
                    else
                        oKullaniciModelKayit.IslemAciklama = "Veri kaydedilemedi";
                }
                else
                {
                    string sSQL =
                        "update public.\"KULLANICI\" " +
                        "set    \"KullaniciAd\" = '" + KullaniciAd + "', " +
                        "       \"Ad\" = '" + Ad + "', " +
                        "       \"Soyad\"='" + Soyad + "', " +
                        "       \"KullaniciTipNo\"=" + KullaniciTipNo + ", " +
                        "       \"AktifPasifTipNo\"=" + AktifPasifTipNo + " ";
                    sSQL += " where \"KullaniciKey\" = " + KullaniciKey;
                    DBUtil.SorguCalistir(sSQL);

                    oKullaniciModelKayit.IslemDurum = "OK";
                }
            }

            return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KullaniciKayitMVC(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit();

                string KullaniciKey = formCollection["KullaniciKey"];
                string KullaniciAd = formCollection["KullaniciAd"];
                string Ad = formCollection["Ad"];
                string Soyad = formCollection["Soyad"];

                if (string.IsNullOrEmpty(KullaniciKey))
                {
                    ModelState.AddModelError("", "Kullanıcı bilgisine ukaşılamadı");
                    return PartialView("KullaniciKayit", oKullaniciModelKayit);
                }
                else if (string.IsNullOrEmpty(KullaniciAd))
                {
                    ModelState.AddModelError("", "Kullanıcı adını giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModelKayit);
                }
                else if (string.IsNullOrEmpty(Ad))
                {
                    ModelState.AddModelError("", "Adı giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModelKayit);
                }
                else if (string.IsNullOrEmpty(Soyad))
                {
                    ModelState.AddModelError("", "Soyadı giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModelKayit);
                }

                if (KullaniciKey == "0")
                {
                    string sSQL =
                        "insert into public.\"KULLANICI\" " +
                        "       (\"KullaniciAd\", \"Ad\", \"Soyad\", \"KullaniciTipNo\", \"AktifPasifTipNo\", \"Parola\", \"UKullaniciKey\", \"UTar\") " +
                        "values ('" + KullaniciAd + "','" + Ad + "','" + Soyad + "', 1, 1, '123', 1, CURRENT_DATE) " +
                        "returning \"KullaniciKey\" ";
                    KullaniciKey = DBUtil.SorguCalistir(sSQL);
                    if (KullaniciKey != "0")
                    {
                        return RedirectToAction("KullaniciOku", "Kullanici", new { area = "Yetki", Durum = "O", Key = KullaniciKey });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Veri kaydedilemedi");
                        return PartialView("KullaniciKayit", oKullaniciModelKayit);
                    }
                }
                else
                {
                    string sSQL =
                        "update public.\"KULLANICI\" " +
                        "set    \"KullaniciAd\" = '" + KullaniciAd + "', " +
                        "       \"Ad\" = '" + Ad + "', " +
                        "       \"Soyad\"='" + Soyad + "' ";
                    sSQL += " where \"KullaniciKey\" = " + KullaniciKey;
                    DBUtil.SorguCalistir(sSQL);

                    return RedirectToAction("KullaniciOku", "Kullanici", new { area = "Yetki", Durum = "O", Key = KullaniciKey });
                }
            }
            else
                return HttpNotFound("1111");
        }


        public PartialViewResult KullaniciOkuXXX()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult KullaniciOkuAng(string Durum, string Key)
        {
            // System.Threading.Thread.Sleep(100);
            KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit()
            {
                IslemDurum = "H",
                IslemAciklama = "Tanımsız hata"
            };

            if (string.IsNullOrEmpty(Key))
            {
                oKullaniciModelKayit.IslemAciklama = "Hatalı parametre";
                return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                DataSet ds = DBUtil.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                    oKullaniciModelKayit = new KullaniciModelKayit()
                    {
                        IslemDurum = Durum,
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        AktifPasifTipNo = AktifPasifTipNo,
                        KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),
                        AktifPasifTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, AktifPasifTipNo)
                    };
                    return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
                }
                else
                    oKullaniciModelKayit.IslemAciklama = "Kullanıcı kaydına ulaşılamadı";
            }

            return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult KullaniciOku(string Durum, string Key)
        {
            KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit()
            {
                IslemDurum = "H",
                IslemAciklama = "Tanımsız hata"
            };

            if (string.IsNullOrEmpty(Durum) && string.IsNullOrEmpty(Key))
            {
                oKullaniciModelKayit.IslemAciklama = "Hatalı parametre";
                return PartialView(oKullaniciModelKayit);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                DataSet ds = DBUtil.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                    oKullaniciModelKayit = new KullaniciModelKayit()
                    {
                        IslemDurum = Durum,
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),

                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),

                        AktifPasifTipNo = AktifPasifTipNo,
                        AktifPasifTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, AktifPasifTipNo)

                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    // return PartialView("KullaniciOku", oKullaniciModelKayit);
                }
                else
                    oKullaniciModelKayit.IslemAciklama = "Kullanıcı kaydına ulaşılamadı";
            }

            return PartialView(oKullaniciModelKayit);
        }


        [HttpGet]
        public JsonResult KullaniciSil(string Key)
        {
            KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit()
            {
                IslemDurum = "H",
                IslemAciklama = "Tanımsız hata"
            };

            // deneme amaçlı
            // System.Threading.Thread.Sleep(2000);
            oKullaniciModelKayit.IslemDurum = "OK";
            return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);

            if (string.IsNullOrEmpty(Key))
            {
                oKullaniciModelKayit.IslemAciklama = "Silinecek kayda ulaşılamadı";
                return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
            }
            else if (Key == "1")
            {
                oKullaniciModelKayit.IslemAciklama = "Sistem yöneticisi silinemez";
                return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string sSQL = "delete from public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                string Sonuc = DBUtil.SorguCalistir(sSQL);
                if (Sonuc == "0")
                    oKullaniciModelKayit.IslemDurum = "OK";
                else
                    oKullaniciModelKayit.IslemAciklama = Sonuc;

                return Json(oKullaniciModelKayit, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public PartialViewResult KullaniciSilMVC(string Durum, string Key)
        {
            KullaniciModelKayit oKullaniciModelKayit = new KullaniciModelKayit()
            {
                IslemDurum = "H"
            };

            if (string.IsNullOrEmpty(Key))
            {
                return PartialView(oKullaniciModelKayit);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                DataSet ds = DBUtil.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    oKullaniciModelKayit = new KullaniciModelKayit()
                    {
                        IslemDurum = Durum,
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo)
                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    return PartialView("KullaniciOku", oKullaniciModelKayit);
                }
            }

            return PartialView(oKullaniciModelKayit);
        }

        public ActionResult KullaniciResimGetir(string Key)
        {
            return File(@"C:\ArgemProje\ISG\ISGWebSite\Resim\Ortak\PersonelResim.png", "image/png");
        }

        #region login işlemleri

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SistemeGirisYap(LoginViewModel oLoginViewModel)
        {
            string Sonuc = "";
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(oLoginViewModel.KullaniciAd) || string.IsNullOrEmpty(oLoginViewModel.Parola))
                    Sonuc = "Kullanıcı adı ve/veya şifre boş";
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
                                Sonuc = "OK";
                            }
                            else
                                Sonuc = "Kullanıcı pasif durumda";
                        }
                        else
                            Sonuc = "Kullanıcı adı ve/veya şifre hatalı";
                    }
                    else
                        Sonuc = "Kullanıcı adı ve/veya şifre hatalı.";
                }
            }
            else
                Sonuc = "Gerekli alanları giriniz";

            return Json(Sonuc, JsonRequestBehavior.DenyGet);
        }

        #endregion

    }
}