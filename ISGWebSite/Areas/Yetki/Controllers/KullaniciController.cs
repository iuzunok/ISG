using Argem.DataServices;
using ArgemUtil;
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
using static ISGWebSite.Areas.Yetki.Models.GenelModel;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    // https://www.youtube.com/watch?v=_r6laMn70FA paging
    public class KullaniciController : BaseController
    {
        public ActionResult YetkiVer()
        {
            return View();
        }


        public ActionResult KullaniciAra()
        {
            return View();
        }


        public JsonResult KullaniciAraSonuc(KullaniciModel oKullaniciModel)
        {
            // System.Threading.Thread.Sleep(2000);
            SonucModel<KullaniciAraModel> oSonucModel = new SonucModel<KullaniciAraModel>() { Durum = "H", Aciklama = "" };

            ArgemSQL oSQL = new ArgemSQL();
            oSQL.CommandText =
                "SELECT * FROM public.\"KULLANICI\"";

            if (!string.IsNullOrEmpty(oKullaniciModel.KullaniciAd))
                oSQL.Gecen("KullaniciAd", oKullaniciModel.KullaniciAd);
            else if (!string.IsNullOrEmpty(oKullaniciModel.Ad))
                oSQL.Gecen("Ad", oKullaniciModel.Ad);
            else if (!string.IsNullOrEmpty(oKullaniciModel.Soyad))
                oSQL.Gecen("Soyad", oKullaniciModel.Soyad);

            oSQL.Esit("KullaniciTipNo", oKullaniciModel.KullaniciTipNo, KolonTipi.Int, false);
            oSQL.Esit("AktifPasifTipNo", oKullaniciModel.AktifPasifTipNo, KolonTipi.Int, false);
            oSQL.OrderByAsc("Ad,Soyad");

            using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
            {
                DataTable dt = new DataTable();
                oData.DataGetir(ref dt, oSQL);

                if (dt.Rows.Count > 0)
                {
                    List<KullaniciAraModel> aryKullaniciAraModel = new List<KullaniciAraModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                        int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                        KullaniciAraModel oKullaniciAraModel = new KullaniciAraModel()
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
                        aryKullaniciAraModel.Add(oKullaniciAraModel);
                    }

                    oSonucModel.Durum = "";
                    oSonucModel.Data = aryKullaniciAraModel;
                }
                else
                    oSonucModel.Aciklama = "Aradığınız kritere uygun kullanıcı kaydı bulunamadı";

                return Json(oSonucModel, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult KullaniciGetir(string KullaniciKey)
        {
            // System.Threading.Thread.Sleep(2000);
            SonucModel<KullaniciModel> oSonucModel = new SonucModel<KullaniciModel>() { Durum = "H", Aciklama = "" };

            if (KullaniciKey == "" || KullaniciKey == "0")
            {
                List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();
                // ilk kayıtta default verilerin dolu gelmesi için 
                KullaniciModel oKullaniciModel = new KullaniciModel()
                {
                    KullaniciKey = 0,
                    KullaniciTipNo = 1,
                    AktifPasifTipNo = 100
                };
                aryKullaniciModel.Add(oKullaniciModel);
                oSonucModel.Durum = "";
                oSonucModel.Data = aryKullaniciModel;
                return Json(oSonucModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ArgemSQL oSQL = new ArgemSQL();
                oSQL.CommandText =
                    "SELECT * FROM public.\"KULLANICI\"";
                oSQL.Esit("KullaniciKey", KullaniciKey, KolonTipi.Int, true);

                using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                {
                    DataTable dt = new DataTable();
                    oData.DataGetir(ref dt, oSQL);

                    if (dt.Rows.Count > 0)
                    {
                        List<KullaniciModel> aryKullaniciModel = new List<KullaniciModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                            int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                            KullaniciModel oKullaniciModel = new KullaniciModel()
                            {
                                KullaniciKey = Convert.ToInt32(dr["KullaniciKey"]),
                                KullaniciAd = dr["KullaniciAd"].ToString(),
                                Ad = dr["Ad"].ToString(),
                                Soyad = dr["Soyad"].ToString(),
                                KullaniciTipNo = KullaniciTipNo,
                                // KullaniciTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, KullaniciTipNo),
                                AktifPasifTipNo = AktifPasifTipNo,
                                // AktifPasifTipNoUzunAd = CacheHelper.LookUzunAdGetir(CacheHelper.DatabaseTipNo.Yetki, AktifPasifTipNo)
                            };
                            aryKullaniciModel.Add(oKullaniciModel);
                        }

                        oSonucModel.Durum = "";
                        oSonucModel.Data = aryKullaniciModel;
                    }

                    return Json(oSonucModel, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult KullaniciKayit(string Durum, string Key)
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult KullaniciKayit(KullaniciModel oKullaniciModel)
        {
            SonucModel<KullaniciModel> oSonucModel = new SonucModel<KullaniciModel>() { Durum = "H", Aciklama = "" };

            int KullaniciKey = oKullaniciModel.KullaniciKey;
            string KullaniciAd = oKullaniciModel.KullaniciAd;
            string Ad = oKullaniciModel.Ad;
            string Soyad = oKullaniciModel.Soyad;
            int KullaniciTipNo = oKullaniciModel.KullaniciTipNo;
            int AktifPasifTipNo = oKullaniciModel.AktifPasifTipNo;

            if (string.IsNullOrEmpty(KullaniciAd))
                oSonucModel.Aciklama = "Kullanıcı adı boş olamaz";
            else if (string.IsNullOrEmpty(Ad))
                oSonucModel.Aciklama = "Ad boş olamaz";
            else if (string.IsNullOrEmpty(Soyad))
                oSonucModel.Aciklama = "Soyad boş olamaz";
            else if (KullaniciTipNo == 0)
                oSonucModel.Aciklama = "Tip boş olamaz";
            else if (AktifPasifTipNo == 0)
                oSonucModel.Aciklama = "Durumu boş olamaz";
            else
            {
                if (KullaniciKey == 0)
                {
                    ArgemSQL oSQL = new ArgemSQL();
                    oSQL.CommandText =
                        "insert into public.\"KULLANICI\" " +
                        "       (\"KullaniciAd\", \"Ad\", \"Soyad\", \"KullaniciTipNo\", \"AktifPasifTipNo\", \"Parola\", \"UKullaniciKey\", \"UTar\") " +
                        "values ('" + KullaniciAd + "','" + Ad + "','" + Soyad + "', " + KullaniciTipNo + ", " + AktifPasifTipNo + ", '123', 1, CURRENT_DATE) " +
                        "returning \"KullaniciKey\" ";
                    using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                    {
                        string SonucYetkiGrupKey = Convert.ToString(oData.SorguCalistir(oSQL));
                        if (SonucYetkiGrupKey != "0")
                            oSonucModel.Durum = "";
                        else
                            oSonucModel.Aciklama = "Veri kaydedilemedi";
                    }
                }
                else
                {
                    ArgemSQL oSQL = new ArgemSQL();
                    oSQL.CommandText =
                        "update public.\"KULLANICI\" " +
                        "set    \"KullaniciAd\" = '" + KullaniciAd + "', " +
                        "       \"Ad\" = '" + Ad + "', " +
                        "       \"Soyad\"='" + Soyad + "', " +
                        "       \"KullaniciTipNo\"=" + KullaniciTipNo + ", " +
                        "       \"AktifPasifTipNo\"=" + AktifPasifTipNo + ", " +
                        "       \"UKullaniciKey\" = " + Session["OpKullaniciKey"] + ", " +
                        "       \"UTar\" = current_timestamp ";
                    oSQL.Esit("KullaniciKey", KullaniciKey, KolonTipi.Int, true);
                    using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                    {
                        oData.SorguCalistir(oSQL);
                        oSonucModel.Durum = "";
                    }
                }
            }

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult KullaniciOku(string Durum, string Key)
        {
            SonucModel<KullaniciAraModel> oSonucModel = new SonucModel<KullaniciAraModel>() { Durum = "H", Aciklama = "" };

            if (string.IsNullOrEmpty(Durum) && string.IsNullOrEmpty(Key))
            {
                oSonucModel.Aciklama = "Hatalı parametre";
                return PartialView(oSonucModel);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                DataSet ds = DBUtilPostger.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int KullaniciTipNo = Convert.ToInt32(dr["KullaniciTipNo"]);
                    int AktifPasifTipNo = Convert.ToInt32(dr["AktifPasifTipNo"]);
                    KullaniciAraModel oKullaniciAraModel = new KullaniciAraModel()
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

                    List<KullaniciAraModel> aryKullaniciAraModel = new List<KullaniciAraModel>();
                    aryKullaniciAraModel.Add(oKullaniciAraModel);

                    oSonucModel.Durum = Durum;
                    oSonucModel.Data = aryKullaniciAraModel;
                }
                else
                    oSonucModel.Aciklama = "Kayıt bulunamadı";
            }

            return PartialView(oSonucModel);
        }

        [HttpGet]
        public ActionResult Kullanici(string Key)
        {
            return KullaniciOku("O", Key);
        }

        [HttpGet]
        public JsonResult KullaniciSil(string Key)
        {
            SonucModel<KullaniciModel> oSonucModel = new SonucModel<KullaniciModel>() { Durum = "H", Aciklama = "" };

            if (string.IsNullOrEmpty(Key))
                oSonucModel.Aciklama = "Silinecek kayda ulaşılamadı";
            else if (Key == "1")
                oSonucModel.Aciklama = "Sistem yöneticisi silinemez";
            {
                string sSQL = "delete from public.\"KULLANICI\"";
                sSQL += " where \"KullaniciKey\" = " + Key;
                string Sonuc = DBUtilPostger.SorguCalistir(sSQL);
                if (Sonuc == "0")
                    oSonucModel.Durum = "";
                else
                    oSonucModel.Aciklama = Sonuc;
            }

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KullaniciResimGetir(string Key)
        {
            return File(@"C:\ArgemProje\ISG\ISGWebSite\Resim\Ortak\PersonelResim.png", "image/png");
        }

        public JsonResult KullaniciGetirDDLText(string AramaKriter)
        {
            // System.Threading.Thread.Sleep(2000);
            SonucModel<DDlTextModel> oSonucModel = new SonucModel<DDlTextModel>() { Durum = "H", Aciklama = "" };

            ArgemSQL oSQL = new ArgemSQL();
            oSQL.CommandText =
                "SELECT * FROM public.\"KULLANICI\"";

            if (!string.IsNullOrEmpty(AramaKriter))
            {
                oSQL.Gecen("Ad", AramaKriter);
                oSQL.OR();
                oSQL.Gecen("Soyad", AramaKriter);
                oSQL.OrderByAsc("Ad,Soyad");

                using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                {
                    DataTable dt = new DataTable();
                    oData.DataGetir(ref dt, oSQL);

                    if (dt.Rows.Count > 0)
                    {
                        List<DDlTextModel> aryDDlTextModel = new List<DDlTextModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            DDlTextModel oDDlTextModel = new DDlTextModel()
                            {
                                Key = Convert.ToInt32(dr["KullaniciKey"]),
                                Text = dr["Ad"].ToString() + " " +dr["Soyad"].ToString()
                            };
                            aryDDlTextModel.Add(oDDlTextModel);
                        }

                        oSonucModel.Durum = "";
                        oSonucModel.Data = aryDDlTextModel;
                    }
                    else
                        oSonucModel.Aciklama = "Aradığınız kritere uygun kullanıcı kaydı bulunamadı";
                }
            }
            else
                oSonucModel.Aciklama = "Kritere uygun kayıt bulunamadı";

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Deneme()
        {
            return View();
        }
    }
}