using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ISGWebSite.Areas.Yetki.Models.Kullanici.LookModel;

namespace ISGWebSite.Models.Yetki.Kullanici
{
    public class KullaniciModelKayit : BaseModel
    {
        public int KullaniciKey { get; set; }

        [Required]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAd { get; set; }


        public int KullaniciTipNo { get; set; }

        [Display(Name = "Kullanıcı Tipi")]
        public string KullaniciTipNoUzunAd { get; set; }

        public List<LookModelDetay> KullaniciTipNolar { get; set; }


        public int AktifPasifTipNo { get; set; }

        [Display(Name = "Aktif/Pasif")]
        public string AktifPasifTipNoUzunAd { get; set; }

        public List<LookModelDetay> AktifPasifTipNolar { get; set; }


        public int UKullaniciKey { get; set; }

        public DateTime UTar { get; set; }
    }

    public class KullaniciModelAra
    {
        public int KullaniciKey { get; set; }

        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAd { get; set; }

        [Display(Name = "Kullanıcı Tipi")]
        public int KullaniciTipNo { get; set; }

        public string KullaniciTipNoUzunAd { get; set; }

        [Display(Name = "Aktif/Pasif")]
        public int AktifPasifTipNo { get; set; }

        public string AktifPasifTipNoUzunAd { get; set; }
    }

}