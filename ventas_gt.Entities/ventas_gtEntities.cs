using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using ventas_gt.Entities.Entities;

namespace ventas_gt.Entities
{

    public partial class ventas_gtEntities : DbContext
    {
        public ventas_gtEntities(string conexion) : base(nameOrConnectionString: conexion) { }

        public DbSet<cuscar_container_info> cuscar_container_info { get; set; }

        public DbSet<aduanas> aduanas { get; set; }

        public DbSet<bill_of_lading> bill_of_lading { get; set; }

        public DbSet<viajes> viajes { get; set; }

        public DbSet<bl_completo> bl_completo { get; set; }

        public DbSet<cuscar_viaje_info> cuscar_viaje_info { get; set; }

        public DbSet<cuscar_bl_info> cuscar_bl_info { get; set; }

        public DbSet<contenedor_completo> contenedor_completo { get; set; }

        public DbSet<viaje_contenedor> viaje_contenedor { get; set; }

        public DbSet<cargos_bl> cargos_bl { get; set; }

        public DbSet<aahist> aahist { get; set; }
    }
}
