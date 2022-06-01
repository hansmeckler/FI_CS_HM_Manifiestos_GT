using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manifiesto.Web.Areas.Maritimo.Models
{
    public class cuscar_bl_infoVM
    {
        public long cuscar_bl_id { get; set; }

        public long cuscar_viaje_id { get; set; }

        public long cuscar_container_id { get; set; }

        public string no_bl { get; set; }

        public Nullable<int> id_shipper { get; set; }

        public Nullable<int> id_cliente { get; set; }

        public string no_piezas { get; set; }

        public Nullable<int> id_tipo_paquete { get; set; }

        public string peso { get; set; }

        public string volumen { get; set; }

        public Nullable<int> id_puerto_embarque { get; set; }

        public Nullable<int> id_destino_final { get; set; }

        public Nullable<long> id_almacen { get; set; }

        public bool dividido { get; set; }

        public string marks_and_numbers { get; set; }

        public Nullable<long> comodity_id { get; set; }

        public string shipper { get; set; }

        public string cliente { get; set; }

        public string puerto_embarque { get; set; }

        public string destino_final { get; set; }

        public string comodity { get; set; }

        public string mbl { get; set; }

        public string ono_bl { get; set; }

        public Nullable<int> xcontenedor_id { get; set; }

        public Nullable<int> puerto_emb_tica_id { get; set; }

        public Nullable<int> puerto_desemb_tica_id { get; set; }

        public Nullable<int> embalaje_tica_id { get; set; }

        public Nullable<int> ubicacion_tica_id { get; set; }

        public string tipo_conocimiento_id { get; set; }

        public string tipo_identificacion_id { get; set; }

        public string observaciones { get; set; }

        public Nullable<int> tipo_merc_p { get; set; }

        public string tipo_paquete { get; set; }

        //public Nullable<int> selected { get; set; }

        public Nullable<int> id_aduana { get; set; }

        public Nullable<int> id_status { get; set; }

        public Nullable<int> bl_id { get; set; }

        public string codigo_tributario { get; set; }

        public Nullable<decimal> flete { get; set; }

        public Nullable<int> tipo_docto { get; set; }

        public Nullable<int> ttm_id { get; set; }

        public string direccion_cliente { get; set; }

        public bool selected { get; set; }

        public string correlativo { get; set; }

        public string no_contenedor { get; set; }

        public string tipo_moneda { get; set; }

        public string aduanagln { get; set; }

        public string puerto_desembarque { get; set; }

        public string flete_val { get; set; }

        public string nit { get; set; }

        public string tipo_docto_desc { get; set; }
    }
}