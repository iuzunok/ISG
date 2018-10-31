using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ISGWebSite.Models
{
    [Table("PERSONEL", Schema = "public")]
    public class Personel
    {
        [Key]
        public int PersonelKey { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int? SicilNo { get; set; }
        public int PersonelTipNo { get; set; }

    }
}