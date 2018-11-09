using ISGWebSite.Controllers;
using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    [Authorize]

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
            if (string.IsNullOrEmpty(Key))
            {
                return PartialView();
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
                    KullaniciModel oKullaniciModel = new KullaniciModel()
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

            return PartialView();
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