using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ISGWebSite.Areas.Yetki.Models.Kullanici.LookModel;

namespace ISGWebSite.Models.Yetki.Kullanici
{
    public class KullaniciModel
    {
        public int KullaniciKey { get; set; }

        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAd { get; set; }

        public int KullaniciTipNo { get; set; }

        //[Display(Name = "Kullanıcı Tipi")]
        // public string KullaniciTipNoUzunAd { get; set; }

        //public List<LookModelDetay> KullaniciTipNolar { get; set; }

        public int AktifPasifTipNo { get; set; }

        //[Display(Name = "Aktif/Pasif")]
        // public string AktifPasifTipNoUzunAd { get; set; }

        //public List<LookModelDetay> AktifPasifTipNolar { get; set; }

        public int UKullaniciKey { get; set; }

        public DateTime UTar { get; set; }
    }

    public class KullaniciAraModel : KullaniciModel
    {
        //[Display(Name = "Kullanıcı Tipi")]
        public string KullaniciTipNoUzunAd { get; set; }

        //[Display(Name = "Aktif/Pasif")]
        public string AktifPasifTipNoUzunAd { get; set; }
    }

   
    public class KullaniciLoginModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAd { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Parola { get; set; }

        [Display(Name = "Kullanıcı adımı hatırla")]
        public bool KullaniciAdimiHatirla { get; set; }
    }


    public class KullaniciYetkiGrupModel
    {
        public int KullaniciYetkiGrupKey { get; set; }
        public int KullaniciKey { get; set; }
        public int YetkiGrupKey { get; set; }
        public int TeskilatKey { get; set; }
    }

    public class KullaniciYetkiGrupAraModel : KullaniciYetkiGrupModel
    {
        public int TeskilatUzunAd { get; set; }
    }

}