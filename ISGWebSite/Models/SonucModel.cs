using System.Collections.Generic;

namespace ISGWebSite.Models
{
    public class SonucModel<T>
    {
        public string Durum { get; set; }
        public string Aciklama { get; set; }
        public List<T> Data { get; set; }
    }
}