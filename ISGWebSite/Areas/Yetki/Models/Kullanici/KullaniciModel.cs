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
        public int KullaniciKey { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KullaniciAd { get; set; }
        public int KullaniciTipNo { get; set; }
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