using ISGWebSite.Models.Yetki.Kullanici;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace CustomAuthenticationMVC.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // public ICollection<Role> Roles { get; set; }

        #endregion

        public CustomMembershipUser(KullaniciModel user) : base("CustomMembership", user.KullaniciAd, user.KullaniciKey, string.Empty, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.KullaniciKey;
            FirstName = user.Ad;
            LastName = user.Soyad;
            // Roles = user.Roles;
        }
    }
}