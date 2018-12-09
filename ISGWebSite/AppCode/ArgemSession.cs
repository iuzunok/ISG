using System;
using System.Web;

namespace ISGWebSite.AppCode
{
    public class ArgemSession
    {
        private static readonly ArgemSession _instance = new ArgemSession();

        private ArgemSession()
        {
        }

        public static int OpKullaniciKey
        {
            get
            {
                if (HttpContext.Current.Session["OpKullaniciKey"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["OpKullaniciKey"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["OpKullaniciKey"] = value;
            }
        }

        public static string OpKullaniciAd { get; set; }

        public static string OpKullaniciSoyad { get; set; }
    }
}