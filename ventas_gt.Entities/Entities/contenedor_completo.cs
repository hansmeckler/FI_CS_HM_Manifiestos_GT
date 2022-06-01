using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("contenedor_completo", Schema = "public")]
    public class contenedor_completo
    {
        [Key]
        public long contenedor_id { get; set; }

        public Nullable<long> bl_id { get; set; }

        public string no_contenedor { get; set; }
        
        public Nullable<long> id_container_type { get; set; }

        public string container_type_tica_id { get; set; }

        public string seal { get; set; }

        public Nullable<Single> no_piezas { get; set; }

        public Nullable<long> id_tipo_paquete { get; set; }

        public Nullable<decimal> peso { get; set; }

        public Nullable<long> id_unidad_peso { get; set; }

        public Nullable<decimal> volumen { get; set; }

        public Nullable<long> id_unidad_volumen { get; set; }

        public Nullable<long> comodity_id { get; set; }

        public Nullable<long> embalaje_tica_id { get; set; }

        public Nullable<int> tipo_merc_p { get; set; }

        public bool warehouse { get; set; }

        public Nullable<DateTime> fecha_ingreso_sistema { get; set; }

        public bool activo { get; set; }

        public string no_docto { get; set; }

        public Nullable<DateTime> ts_ingreso_sistema { get; set; }

        public string observacion { get; set; }

        public Nullable<int> inventario_tica { get; set; }

        public Nullable<DateTime> fecha_retorno { get; set; }

        public string mbl { get; set; }

        public Nullable<long> agente_id { get; set; }

        public Nullable<long> bl_id_ref { get; set; }
        
    }
}
