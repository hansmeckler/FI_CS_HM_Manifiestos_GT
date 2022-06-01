using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using db_aereo.Entities.Entities;

namespace db_aereo.Entities
{
    public partial class db_aereoContext : DbContext
    {
        public db_aereoContext() : base(nameOrConnectionString: "db_aereo") { }

        public DbSet<awbi> guia_import { get; set; }

        public DbSet<carriers> Carriers { get; set; }

        public DbSet<awb> guia_export { get; set; }

        public DbSet<cuscar_voyage_info> cuscar_voyage_info { get; set; }

        public DbSet<cuscar_bl_info> cuscar_bl_info { get; set; }

        public DbSet<Airports> Airports { get; set; }
    }
}
