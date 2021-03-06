using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace db_aereo.Entities.Entities
{
    [Table("cuscar_bl_info")]
    public class cuscar_bl_info
    {
        [Key]
        public int cuscar_bl_id { get; set; }
        public Nullable<int> cuscar_voyage_id { get; set; }
        public string no_bl { get; set; }
        public Nullable<int> id_shipper { get; set; }
        public Nullable<int> id_cliente { get; set; }
        public Nullable<int> no_piezas { get; set; }
        public Nullable<int> id_tipo_paquete { get; set; }
        public Nullable<double> peso { get; set; }
        public string volumen { get; set; }
        public Nullable<int> id_puerto_embarque { get; set; }
        public Nullable<int> id_destino_final { get; set; }
        public string comodity_id { get; set; }
        public string shipper { get; set; }
        public string cliente { get; set; }
        public string direccion { get; set; }
        public string puerto_embarque { get; set; }
        public string destino_final { get; set; }
        public string comodity { get; set; }
        public string mbl { get; set; }
        public Nullable<int> bl_id1 { get; set; }
        public Nullable<int> contenedor_id { get; set; }
        public string codigo_tributario { get; set; }
        public Nullable<double> flete { get; set; }
        public Nullable<int> tipo_docto { get; set; }
        public Nullable<int> ttm_id { get; set; }
    }
}
