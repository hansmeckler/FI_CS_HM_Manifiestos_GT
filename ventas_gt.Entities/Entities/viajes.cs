using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("viajes", Schema = "public")]
    public class viajes
    {
        [Key]
        public long viaje_id { get; set; }

        public Nullable<long> user_id { get; set; }

        public string no_viaje { get; set; }

        public Nullable<DateTime> fecha_arribo { get; set; }

        public Nullable<DateTime> fecha_ingreso_sistema { get; set; }

        public string vapor { get; set; }

        public Nullable<long> agente_id { get; set; }

        public bool activo { get; set; }

        public string no_remision { get; set; }

        public Nullable<DateTime> fecha_salida { get; set; }

        public string voyage { get; set; }

        public Nullable<long> id_naviera { get; set; }

        public Nullable<long> id_puerto_origen { get; set; }

        public Nullable<long> id_puerto_desembarque { get; set; }

        public Nullable<DateTime> eta { get; set; }

        public Nullable<DateTime> etd { get; set; }

        public string pais_orig_id { get; set; }

        public Nullable<DateTime> eta_almacen { get; set; }

        public bool import_export { get; set; }

        public Nullable<DateTime> ts_ingreso_sistema { get; set; }

        public Nullable<long> id_deposito { get; set; }

    }
}
