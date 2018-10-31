using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ISGWebSite.Models
{
    public class PGDbContext : DbContext
    {
        public PGDbContext() : base(nameOrConnectionString: "PersonelConnectionString")
        {
            // Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Personel> Personeller { get; set; }
    }
}