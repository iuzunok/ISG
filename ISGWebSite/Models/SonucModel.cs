using System.Collections.Generic;

namespace ISGWebSite.Models
{
    public class SonucModelBase
    {
        public string Durum { get; set; }
        public string Aciklama { get; set; }

        public int ToplamKayitAdet { get; set; }

        /// <summary>
        /// server çalışma süresi
        /// </summary>
        public string SCS { get; set; }
    }

    public class SonucModel<T> : SonucModelBase
    {
        public List<T> Data { get; set; }
    }


}