using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISGWebSite.Models.Yetki.Kullanici
{
    public class KullaniciModel
    {
        public string IslemDurum { get; set; }

        public int KullaniciKey { get; set; }

        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAd { get; set; }

        public int KullaniciTipNo { get; set; }

        [Display(Name = "Kullanıcı Tipi")]
        public string  KullaniciTipNoUzunAd { get; set; }
        public int UKullaniciKey { get; set; }
        public DateTime UTar { get; set; }
    }
         
    public class CustomSerializeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> RoleName { get; set; }
    }







}