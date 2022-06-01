using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("cuscar_viaje_info", Schema = "public")]
    public class cuscar_viaje_info
    {
        [Key]
        public long cuscar_viaje_id { get; set; }

        public Nullable<long> viaje_id { get; set; }

        public string no_viaje { get; set; }

        public string vapor { get; set; }

        public Nullable<int> id_naviera { get; set; }

        public Nullable<int> id_puerto_origen { get; set; }

        public Nullable<int> id_puerto_desembarque { get; set; }

        public Nullable<int> agente { get; set; }

        public Nullable<int> user_id { get; set; }

        public string naviera { get; set; }

        public string puerto_origen { get; set; }

        public string puerto_desembarque { get; set; }

        public string tipo { get; set; }

        public Nullable<DateTime> fecha_salida { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> fecha_arribo { get; set; }

        public Nullable<DateTime> eta { get; set; }

        public Nullable<DateTime> etd { get; set; }

        public Nullable<bool> import_export { get; set; }

        public Nullable<long> id_usuario { get; set; }

        public Nullable<DateTime> date_created { get; set; }

        public Nullable<int> id_deposito { get; set; }
    }
}
