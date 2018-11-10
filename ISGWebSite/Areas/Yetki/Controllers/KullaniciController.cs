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
        //// GET: Yetki/Kullanici
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult KullaniciKayit(string Durum, string Key)
        {
            KullaniciModel oKullaniciModel = new KullaniciModel()
            {
                IslemDurum = "H"
            };

            if (string.IsNullOrEmpty(Durum) && string.IsNullOrEmpty(Key))
            {
                return PartialView(oKullaniciModel);
            }
            else if (Durum == "I")
            {
                oKullaniciModel = new KullaniciModel()
                {
                    IslemDurum = "I",
                    KullaniciKey = 0,
                    KullaniciAd = "",
                    Ad = "",
                    Soyad = "",
                    KullaniciTipNo = 0,
                    KullaniciTipNoUzunAd = ""
                    // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                    // UTar = Convert.ToDateTime(dr["UTar"])
                };
                return PartialView("KullaniciKayit", oKullaniciModel);
            }
            else if (string.IsNullOrEmpty(Key))
            {
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
                    oKullaniciModel = new KullaniciModel()
                    {
                        IslemDurum = "U",
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = UzunAdBul(KullaniciTipNo)
                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    return PartialView("KullaniciKayit", oKullaniciModel);
                }
            }

            return PartialView(oKullaniciModel);
        }

        public ActionResult KullaniciKayit(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                KullaniciModel oKullaniciModel = new KullaniciModel();

                string KullaniciKey = formCollection["KullaniciKey"];
                string KullaniciAd = formCollection["KullaniciAd"];
                string Ad = formCollection["Ad"];
                string Soyad = formCollection["Soyad"];

                if (string.IsNullOrEmpty(KullaniciKey))
                {
                    ModelState.AddModelError("", "Kullanıcı bilgisine ukaşılamadı");
                    return PartialView("KullaniciKayit", oKullaniciModel);
                }
                else if (string.IsNullOrEmpty(KullaniciAd))
                {
                    ModelState.AddModelError("", "Kullanıcı adını giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModel);
                }
                else if (string.IsNullOrEmpty(Ad))
                {
                    ModelState.AddModelError("", "Adı giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModel);
                }
                else if (string.IsNullOrEmpty(Soyad))
                {
                    ModelState.AddModelError("", "Soyadı giriniz");
                    return PartialView("KullaniciKayit", oKullaniciModel);
                }

                if (KullaniciKey == "0")
                {
                    string sSQL =
                        "insert into public.\"KULLANICI\" " +
                        "       (\"KullaniciAd\", \"Ad\", \"Soyad\", \"KullaniciTipNo\", \"AktifPasifTipNo\", \"Sifre\", \"UKullaniciKey\", \"UTar\") " +
                        "values ('" + KullaniciAd + "','" + Ad + "','" + Soyad + "', 1, 1, '123', 1, CURRENT_DATE) " +
                        "returning \"KullaniciKey\" ";
                    KullaniciKey = DBUtil.VeriKaydet(sSQL);
                    if (KullaniciKey != "0")
                    {
                        return RedirectToAction("KullaniciOku", "Kullanici", new { area = "Yetki", Durum = "O", Key = KullaniciKey });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Veri kaydedilemedi");
                        return PartialView("KullaniciKayit", oKullaniciModel);
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
                    DBUtil.VeriKaydet(sSQL);

                    return RedirectToAction("KullaniciOku", "Kullanici", new { area = "Yetki", Durum = "O", Key = KullaniciKey });
                }
            }
            else
                return HttpNotFound("1111");
        }

        public ActionResult KullaniciAra(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                // hata oluşturmak için
                // string s = "controller seviye hata";
                // int i = Convert.ToInt16(s);

                List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();

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
                }
                // else
                // TempData["Sonuc"] = "Kayıt bulunamadı";

                return View(aryKullaniciModel);
            }
            return View("sdsdfds");
        }


        [HttpGet]
        public PartialViewResult KullaniciOku(string Durum, string Key)
        {
            KullaniciModel oKullaniciModel = new KullaniciModel()
            {
                IslemDurum = "H"
            };

            if (string.IsNullOrEmpty(Key))
            {
                return PartialView(oKullaniciModel);
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
                    oKullaniciModel = new KullaniciModel()
                    {
                        IslemDurum = Durum,
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = UzunAdBul(KullaniciTipNo)
                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    return PartialView("KullaniciOku", oKullaniciModel);
                }
            }

            return PartialView(oKullaniciModel);
        }


        [HttpGet]
        public PartialViewResult KullaniciSil(string Durum, string Key)
        {
            KullaniciModel oKullaniciModel = new KullaniciModel()
            {
                IslemDurum = "H"
            };

            if (string.IsNullOrEmpty(Key))
            {
                return PartialView(oKullaniciModel);
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
                    oKullaniciModel = new KullaniciModel()
                    {
                        IslemDurum = Durum,
                        KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                        KullaniciAd = dr["KullaniciAd"].ToString(),
                        Ad = dr["Ad"].ToString(),
                        Soyad = dr["Soyad"].ToString(),
                        KullaniciTipNo = KullaniciTipNo,
                        KullaniciTipNoUzunAd = UzunAdBul(KullaniciTipNo)
                        // UKullaniciKey = Convert.ToInt32(dr["UKullaniciKey"]),
                        // UTar = Convert.ToDateTime(dr["UTar"])
                    };
                    return PartialView("KullaniciOku", oKullaniciModel);
                }
            }

            return PartialView(oKullaniciModel);
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