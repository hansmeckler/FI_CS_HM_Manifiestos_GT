using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("viaje_contenedor", Schema = "public")]
    public class viaje_contenedor
    {
        [Key]
        public long viaje_contenedor_id { get; set; }

        public Nullable<long> viaje_id { get; set; }

        public string no_contenedor { get; set; }

        public Nullable<Single> total_cbm { get; set; }

        public Nullable<Single> total_ctns { get; set; }

        public string mbl { get; set; }

        public string declaracion_aduanal { get; set; }

        public Nullable<DateTime> fecha_descarga { get; set; }

        public Nullable<DateTime> fecha_entrega_almacen { get; set; }

        public Nullable<DateTime> fecha_ingreso_sistema { get; set; }

        public bool activo { get; set; }

        public string no_aduana { get; set; }

        public bool mbl_prepaid { get; set; }

        public string seal { get; set; }

        public string container_type_tica_id { get; set; }

        public bool warehouse { get; set; }

        public Nullable<long> id_container_type { get; set; }

        public string coment_arribo_gral { get; set; }

        public string no_manifiesto { get; set; }

        public bool mbl_pendiente { get; set; }

        public DateTime ts_ingreso_sistema { get; set; }

        public string observacion { get; set; }

        public string manifiesto_master { get; set; }

        public bool cerrado { get; set; }

        public Nullable<DateTime> cerrado_fecha { get; set; }
    }
}
