using Argem.DataServices;
using ArgemUtil;
using ISGWebSite.Models;
using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class KullaniciYetkiGrupController : Controller
    {
        public JsonResult KullaniciYetkiGrupAraSonuc(KullaniciYetkiGrupModel oKullaniciYetkiGrupModel)
        {
            SonucModel<KullaniciYetkiGrupAraModel> oSonucModel = new SonucModel<KullaniciYetkiGrupAraModel>() { Durum = "H", Aciklama = "" };

            ArgemSQL oSQL = new ArgemSQL();
            oSQL.CommandText =
                "SELECT * FROM public.\"KULLANICI_YETKI_GRUP\"";
            if (oKullaniciYetkiGrupModel.KullaniciKey > 0)
            {
                oSQL.Esit("KullaniciKey", oKullaniciYetkiGrupModel.KullaniciKey, KolonTipi.Int, true);

                using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                {
                    DataTable dt = new DataTable();
                    oData.DataGetir(ref dt, oSQL);

                    if (dt.Rows.Count > 0)
                    {
                        List<KullaniciYetkiGrupAraModel> arySonuc = new List<KullaniciYetkiGrupAraModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            KullaniciYetkiGrupAraModel oKullaniciYetkiGrupAraModel = new KullaniciYetkiGrupAraModel()
                            {
                                KullaniciYetkiGrupKey = Convert.ToInt32(dr["YetkiGrupKey"]),
                                // KullaniciKey = Convert.ToInt32(dr["YetkiGrupKey"]), // gerek yok sayfada zaten kullanıcı bazında getiriliyor
                                YetkiGrupKey = Convert.ToInt32(dr["YetkiGrupKey"]),
                                YetkiGrupAd = "Sonra koy",
                                TeskilatKey = Convert.ToInt32(dr["YetkiGrupKey"]),
                                TeskilatUzunAd = "ARGEM sonra konulacak"
                            };
                            arySonuc.Add(oKullaniciYetkiGrupAraModel);
                        }

                        oSonucModel.Durum = "";
                        oSonucModel.Data = arySonuc;
                    }
                    else
                        oSonucModel.Aciklama = "Aradığınız kritere uygun kayıt bulunamadı";
                }
            }
            else
                oSonucModel.Aciklama = "Kullanıcı bilgisi bulunamadı";

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KullaniciYetkiGrupKayit(string Durum, string Key, string KullaniciKey)
        {
            return PartialView();
        }

        public JsonResult KullaniciYetkiGrupKayitKaydet(string KullaniciKey, string TeskilatKey, string YetkiGrupKeyler)
        {
            SonucModelBase oSonucModel = new SonucModelBase() { Durum = "H", Aciklama = "" };

            if (string.IsNullOrEmpty(KullaniciKey))
                oSonucModel.Aciklama = "Hatalı parametre (K)";
            else if (string.IsNullOrEmpty(TeskilatKey))
                oSonucModel.Aciklama = "Hatalı parametre (T)";
            else if (string.IsNullOrEmpty(TeskilatKey))
                oSonucModel.Aciklama = "Hatalı parametre (YG)";
            else
            {
                string[] aryYetkiGrupKeyler = YetkiGrupKeyler.Split(',');
                foreach (string YetkiGrupKey in aryYetkiGrupKeyler)
                {
                    ArgemSQL oSQL = new ArgemSQL();
                    oSQL.CommandText =
                        "insert into public.\"KULLANICI_YETKI_GRUP\" " +
                        "       (\"KullaniciKey\", \"YetkiGrupKey\", \"TeskilatKey\", \"UKullaniciKey\", \"UTar\") " +
                        "values (" + KullaniciKey + "," + YetkiGrupKey + "," + TeskilatKey + ", 1, current_timestamp) " +
                        "returning \"KullaniciYetkiGrupKey\" ";
                    using (DBUtil2 oData = new DBUtil2(DataBaseTipi.Yetki))
                    {
                        string SonucKullaniciYetkiGrupKey = Convert.ToString(oData.SorguCalistir(oSQL));
                        if (SonucKullaniciYetkiGrupKey != "0")
                            oSonucModel.Durum = "";
                        else
                            oSonucModel.Aciklama = "Veri kaydedilemedi";
                    }
                }
            }

            return Json(oSonucModel, JsonRequestBehavior.AllowGet);
        }

    }

}
