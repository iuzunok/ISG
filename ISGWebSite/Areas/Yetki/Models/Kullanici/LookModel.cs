using System.ComponentModel.DataAnnotations;

namespace ISGWebSite.Areas.Yetki.Models.Kullanici
{
    public class LookModel
    {
        public class LookModelDetay
        {
            public int LookNo { get; set; }

            //[Display(Name = "AlanAd")]
            //public string AlanAd { get; set; }

            [Display(Name = "UzunAd")]
            public string UzunAd { get; set; }
        }

    }
}