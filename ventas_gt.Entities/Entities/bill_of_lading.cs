using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("bill_of_lading", Schema = "public")]
    public class bill_of_lading
    {
        [Key]
        public long bl_id { get; set; }

        public string no_bl { get; set; }

        public Nullable<long> id_shipper { get; set; }

        public Nullable<long> id_cliente { get; set; }

        public Nullable<long> no_piezas { get; set; }

        public Nullable<long> id_tipo_paquete { get; set; }

        public Nullable<Decimal> peso { get; set; }

        public Nullable<Decimal> volumen { get; set; }

        public Nullable<long> viaje_contenedor_id { get; set; }

        public bool activo { get; set; }

        public string comentario_arribo { get; set; }

        public string observaciones { get; set; }

        public string tipo_conocimiento_id { get; set; }

        public string tipo_identificacion_id { get; set; }

        public Nullable<int> tipo_merc_p { get; set; }

        public string seqnum { get; set; }

        public string marks_and_numbers { get; set; }

        public Nullable<long> id_routing { get; set; }

        public Nullable<long> comodity_id { get; set; }

        public Nullable<long> factura_id { get; set; }

        public Nullable<long> id_destino_final { get; set; }

        public Nullable<long> id_almacen { get; set; }

        public Nullable<long> id_puerto_embarque { get; set; }

        public Nullable<long> id_unidad_peso { get; set; }

        public Nullable<long> id_unidad_volumen { get; set; }

        public string comodity_endoso { get; set; }

        public string endosar_a { get; set; }

        public string id_pais_final { get; set; }

        public long id_aduana { get; set; }

        public Nullable<long> id_cargo { get; set; }

        public DateTime ts_ingreso_sistema { get; set; }

        public Nullable<int> rep_exp { get; set; }

        public Nullable<int> inventario_tica { get; set; }

        public string routing { get; set; }

        public Nullable<Decimal> landing_fee { get; set; }

        public bool en_intermodal { get; set; }

        public string id_pais_final2 { get; set; }

        public string no_bl2 { get; set; }

        public string documents { get; set; }

        public Nullable<int> id_colectar { get; set; }

        public Nullable<long> id_coloader { get; set; }

        public Nullable<decimal> valor_flete_manifestar { get; set; }

        public bool valor_flete_manual { get; set; }

        public DateTime fecha_ingreso_sistema { get; set; }
    }
}
