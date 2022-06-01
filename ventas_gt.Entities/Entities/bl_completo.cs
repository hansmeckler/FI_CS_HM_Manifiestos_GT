using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("bl_completo", Schema = "public")]
    public class bl_completo
    {
        [Key]
        public long bl_id { get; set; }

        public Nullable<long> user_id { get; set; }

        public string no_bl { get; set; }

        public string mbl { get; set; }

        public Nullable<long> id_shipper { get; set; }

        public Nullable<long> id_cliente { get; set; }

        public Nullable<long> id_naviera { get; set; }

        public Nullable<long> agente_id { get; set; }

        public string vapor { get; set; }

        public string no_viaje { get; set; }

        public bool ruteado { get; set; }

        public Nullable<DateTime> eta { get; set; }

        public Nullable<DateTime> fecha_arribo { get; set; }

        public bool liberado { get; set; }

        public string comentario_liberacion { get; set; }

        public bool activo { get; set; }

        public string no_remision { get; set; }

        public Nullable<long> puerto_emb_tica_id { get; set; }

        public Nullable<long> puerto_desemb_tica_id { get; set; }

        public Nullable<long> ubicacion_tica_id { get; set; }

        public string comentario_arribo { get; set; }

        public Nullable<DateTime> etd { get; set; }

        public string no_aduana { get; set; }

        public Nullable<DateTime> fecha_descarga { get; set; }

        public string observaciones { get; set; }

        public bool mbl_prepaid { get; set; }

        public string tipo_conocimiento_id { get; set; }

        public string tipo_identificacion_id { get; set; }

        public string notify_1 { get; set; }

        public string notify_2 { get; set; }

        public string notify_3 { get; set; }

        public string document_no { get; set; }

        public string booking_no { get; set; }

        public string pais_origen_carga { get; set; }

        public string place_of_receipt { get; set; }

        public string loading_pier { get; set; }

        public string terminal { get; set; }

        public string export_reference_1 { get; set; }

        public string export_reference_2 { get; set; }

        public string lugar_impresion_bl { get; set; }

        public Nullable<DateTime> fecha_impresion_bl { get; set; }

        public bool arrival_notice_generated { get; set; }

        public Nullable<long> id_routing { get; set; }

        public bool cerrado { get; set; }

        public Nullable<int> factura_id { get; set; }

        public Nullable<long> id_puerto_embarque { get; set; }

        public Nullable<long> id_puerto_desembarque { get; set; }

        public Nullable<long> id_destino_final { get; set; }

        public Nullable<long> id_almacen { get; set; }

        public Nullable<long> id_puerto_origen { get; set; }

        public Nullable<Single> profit_aimar { get; set; }

        public Nullable<DateTime> eta_almacen { get; set; }

        public bool import_export { get; set; }

        public string no_manifiesto { get; set; }

        public bool en_transito { get; set; }

        public Nullable<long> id_aduana { get; set; }

        public string id_pais_final { get; set; }

        public bool mbl_pendiente { get; set; }

        public Nullable<int> rep_exp { get; set; }

        public Nullable<int> inventario_tica { get; set; }

        public string routing { get; set; }

        public Nullable<int> id_incoterms { get; set; }

        public string manifiesto_master { get; set; }

        public string no_bl2 { get; set; }

        public Nullable<int> id_colectar { get; set; }

        public Nullable<int> agente_secuencia_no { get; set; }

        public string agente_secuencia { get; set; }

        public Nullable<DateTime> cerrado_fecha { get; set; }

        public Nullable<long> bl_id_ref { get; set; }

        public Nullable<int> no_piezas { get; set; }

        public Nullable<int> id_tipo_paquete { get; set; }

        public Nullable<decimal> peso { get; set; }

        public Nullable<int> id_unidad_peso { get; set; }

        public Nullable<decimal> volumen { get; set; }

        public Nullable<int> id_unidad_volumen { get; set; }

        public Nullable<int> comodity_id { get; set; }

        public Nullable<long> id_coloader { get; set; }

        public Nullable<decimal> valor_flete_manifestar { get; set; }

        public Nullable<long> id_deposito { get; set; }

        public bool valor_flete_manual { get; set; }

        public DateTime fecha_ingreso_sistema { get; set; }
    }
}
