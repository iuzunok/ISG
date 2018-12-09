using ISGWebSite.Models;

namespace ISGWebSite.Areas.Yetki.Models.Kullanici
{
    public class YetkiGrupModel : BaseModel
    {
        public int YetkiGrupKey { get; set; }
        public string YetkiGrupAd { get; set; }

        /// <summary>
        /// ekranda seçili/seçili olmayan kayıtlar için
        /// </summary>
        public int S { get; set; }
    }
}