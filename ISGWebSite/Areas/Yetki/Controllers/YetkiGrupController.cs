using Argem.DataServices;
using ArgemUtil;
using ISGWebSite.AppCode;
using ISGWebSite.Areas.Yetki.Models.Kullanici;
using ISGWebSite.Controllers;
using ISGWebSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using VeriTabani;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class YetkiGrupController : BaseController
    {
        public ActionResult YetkiGrupAra()
        {
            return View();
        }

        public JsonResult YetkiGrupAraSonuc(YetkiGrupModel oYetkiGrupModel)
        {
            SonucModel<YetkiGrupModel> oSonucModel = new SonucModel<YetkiGrupModel>() { Durum = "H", Aciklama = "" };

            ArgemSQL oSQL = new ArgemSQL();
            oSQL.CommandText =
                "SELECT * FROM public.\"YETKI_GRUP\"";
            if (oYetkiGrupModel.YetkiGrupKey != 0)
                oSQL.Esit("YetkiGrupKey", oYetkiGrupModel.YetkiGrupKey, KolonTipi.Int, true);
            else if (!string.IsNullOrEmpty(oYetkiGrupModel.YetkiGrupAd))
                oSQL.Gecen("YetkiGrupAd", oYetkiGrupModel.YetkiGrupAd);
            oSQL.OrderByAsc("YetkiGrupAd");

            using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
            {
                DataTable dt = new DataTable();
                oData.DataGetir(ref dt, oSQL);

                if (dt.Rows.Count > 0)
                {
                    List<YetkiGrupModel> aryYetkiGrupModel = new List<YetkiGrupModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        oYetkiGrupModel = new YetkiGrupModel()
                        {
                            YetkiGrupKey = Convert.ToInt32(dr["YetkiGrupKey"]),
                            YetkiGrupAd = dr["YetkiGrupAd"].ToString(),
                            S = 0
                        };
                        aryYetkiGrupModel.Add(oYetkiGrupModel);
                    }

                    oSonucModel.Durum = "";
                    oSonucModel.Data = aryYetkiGrupModel;
                }
                else
                    oSonucModel.Aciklama = "Aradığınız kritere uygun kayıt bulunamadı";

                return Json(oSonucModel, JsonRequestBehavior.AllowGet);
            }           
        }

        [HttpGet]
        public ActionResult YetkiGrupKayit(string Durum, string Key)
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult YetkiGrupKayit(YetkiGrupModel oYetkiGrupModel)
        {
            SonucModel<YetkiGrupModel> oSonucModel = new SonucModel<YetkiGrupModel>() { Durum = "H", Aciklama = "" };

            int YetkiGrupKey = oYetkiGrupModel.YetkiGrupKey;
            string YetkiGrupAd = oYetkiGrupModel.YetkiGrupAd;

            if (string.IsNullOrEmpty(YetkiGrupAd))
                oSonucModel.Aciklama = "Yetki grup adı boş olamaz";
            else
            {
                if (YetkiGrupKey == 0)
                {
                    ArgemSQL oSQL = new ArgemSQL();
                    oSQL.CommandText =
                        "insert into public.\"YETKI_GRUP\" " +
                        "       (\"YetkiGrupAd\", \"UKullaniciKey\", \"UTar\") " +
                        "values ('" + YetkiGrupAd + "'," + ArgemSession.OpKullaniciKey + ", current_timestamp) " +
                        "returning \"YetkiGrupKey\" ";

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
                        "update public.\"YETKI_GRUP\" " +
                        "set    \"YetkiGrupAd\" = '" + YetkiGrupAd + "', " +
                        "       \"UKullaniciKey\" = " + ArgemSession.OpKullaniciKey + ", " +
                        "       \"UTar\" = current_timestamp ";
                    oSQL.Esit("YetkiGrupKey", YetkiGrupKey, KolonTipi.Int, true);
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
        public PartialViewResult YetkiGrupOku(string Durum, string Key)
        {
            SonucModel<YetkiGrupModel> oSonucModel = new SonucModel<YetkiGrupModel>() { Durum = "H", Aciklama = "" };

            if (string.IsNullOrEmpty(Durum) && string.IsNullOrEmpty(Key))
            {
                oSonucModel.Aciklama = "Hatalı parametre";
                return PartialView(oSonucModel);
            }
            else
            {
                string sSQL = "SELECT * FROM public.\"YETKI_GRUP\"";
                sSQL += " where \"YetkiGrupKey\" = " + Key;
                DataSet ds = DBUtilPostger.VeriGetirDS(sSQL);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    YetkiGrupModel oYetkiGrupModel = new YetkiGrupModel()
                    {
                        YetkiGrupKey = Convert.ToInt32(dr["YetkiGrupKey"]),
                        YetkiGrupAd = dr["YetkiGrupAd"].ToString()
                    };

                    List<YetkiGrupModel> aryYetkiGrupModel = new List<YetkiGrupModel>();
                    aryYetkiGrupModel.Add(oYetkiGrupModel);

                    oSonucModel.Durum = Durum;
                    oSonucModel.Data = aryYetkiGrupModel;
                }
                else
                    oSonucModel.Aciklama = "Kayıt bulunamadı";
            }

            return PartialView(oSonucModel);
        }

        [HttpGet]
        public JsonResult YetkiGrupSil(string Key)
        {
            SonucModel<YetkiGrupModel> oSonucModel = new SonucModel<YetkiGrupModel>() { Durum = "H", Aciklama = "" };

            if (string.IsNullOrEmpty(Key))
                oSonucModel.Aciklama = "Silinecek kayda ulaşılamadı";
            else
            {
                string sSQL = "delete from public.\"YETKI_GRUP\"";
                sSQL += " where \"YetkiGrupKey\" = " + Key;
                string Sonuc = DBUtilPostger.SorguCalistir(sSQL);
                if (Sonuc == "0")
                    oSonucModel.Durum = "";
                else
                    oSonucModel.Aciklama = Sonuc;
            }

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }

    }
}