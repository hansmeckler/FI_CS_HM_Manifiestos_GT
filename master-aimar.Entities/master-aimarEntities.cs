using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using master_aimar.Entities.Entities;

namespace master_aimar.Entities
{
    public partial class master_aimarEntities : DbContext
    {

        public master_aimarEntities() : base(nameOrConnectionString: "master-aimar") { }

        public DbSet<usuarios_empresas> usuarios_empresas { get; set; }

        public DbSet<barcos> barcos { get; set; }

        public DbSet<container_type> container_type { get; set; }

        public DbSet<clientes> clientes { get; set; }

        public DbSet<tipo_paquete> tipo_paquete { get; set; }

        public DbSet<navieras> navieras { get; set; }

        public DbSet<unlocode> unlocode { get; set; }

        public DbSet<commodities> commodities { get; set; }

        public DbSet<direcciones> direcciones { get; set; }

        public DbSet<cuscar_users> cuscar_users { get; set; }

        public DbSet<empresas> empresas { get; set; }

        public DbSet<usuarios_x_empresa> usuarios_x_empresa { get; set; }

        public DbSet<usuarios_permisos_manifiestos> usuarios_permisos_manifiestos { get; set; }

        public DbSet<paises> paises { get; set; }

        public DbSet<region> region { get; set; }

        public DbSet<aereo_temp> aereo_temp { get; set; }
    }

    /*
    internal sealed class Configuration : DbMigrationsConfiguration<DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
    */
}
