using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using VeriTabani;
using static ISGWebSite.Areas.Yetki.Models.Kullanici.LookModel;

namespace ISGWebSite.AppCode
{
    public class CacheHelper
    {
        public enum DatabaseTipNo
        {
            Yetki = 1,
            Personel = 2
        }

        public static DataSet YetkiLookGetir()
        {
            DataSet ds;
            if (HttpRuntime.Cache["ARGEM_YETKI"] == null)
            {
                string sSQL = "SELECT \"LookNo\", \"AlanAd\", \"UzunAd\" FROM public.\"YETKI_LOOK\"";
                ds = DBUtilPostger.VeriGetirDS(sSQL);
                HttpRuntime.Cache["ARGEM_YETKI"] = ds;
            }
            return (DataSet)HttpRuntime.Cache["ARGEM_YETKI"];
        }

        public static string LookUzunAdGetir(DatabaseTipNo enumDatabaseTipNo, object LookNo)
        {
            DataSet ds = new DataSet();
            if (enumDatabaseTipNo == DatabaseTipNo.Yetki)
                ds = YetkiLookGetir();

            DataRow[] aryDr = ds.Tables[0].Select("LookNo=" + LookNo.ToString());
            if (aryDr.Length > 0)
                return aryDr[0]["UzunAd"].ToString();
            else
                return "";
        }

        public static List<LookModelDetay> LookGetir(DatabaseTipNo enumDatabaseTipNo, string AlanAd)
        {
            DataSet ds = new DataSet();
            if (enumDatabaseTipNo == DatabaseTipNo.Yetki)
                ds = YetkiLookGetir();

            List<LookModelDetay> aryLookModelDetay = new List<LookModelDetay>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["AlanAd"].ToString() == AlanAd)
                {
                    LookModelDetay oLookModelDetay = new LookModelDetay()
                    {
                        LookNo = Convert.ToInt32(dr["LookNo"]),
                        UzunAd = dr["UzunAd"].ToString()
                    };
                    aryLookModelDetay.Add(oLookModelDetay);
                }
            }
            return aryLookModelDetay;
        }

    }
}