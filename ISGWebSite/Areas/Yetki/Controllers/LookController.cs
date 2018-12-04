using ISGWebSite.AppCode;
using ISGWebSite.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using static ISGWebSite.Areas.Yetki.Models.Kullanici.LookModel;

namespace ISGWebSite.Areas.Yetki.Controllers
{
    public class LookController : BaseController
    {
        [OutputCache(Duration = 0)]
        public JsonResult LookGetir(string TabloAd, string AlanAd)
        {
            CacheHelper.DatabaseTipNo enumDatabaseTipNo = (CacheHelper.DatabaseTipNo)Enum.Parse(typeof(CacheHelper.DatabaseTipNo), TabloAd);
            // Enum.TryParse(TabloAd, out CacheHelper.DatabaseTipNo enumDatabaseTipNo);
            List<LookModelDetay> aryLookModelDetay = CacheHelper.LookGetir(enumDatabaseTipNo, AlanAd);
            return Json(aryLookModelDetay, JsonRequestBehavior.AllowGet);
        }

    }
}