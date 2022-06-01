using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ventas_gt.Entities.Entities
{
    [Table("cargos_bl", Schema = "public")]
    public class cargos_bl
    {
        [Key]
        public long cargo_id { get; set; }

        public long bl_id { get; set; }

        public string tipo_bl { get; set; }

        public Nullable<long> id_moneda { get; set; }

        public Nullable<long> id_rubro { get; set; }

        public string detalle { get; set; }

        public Nullable<decimal> valor_prepaid { get; set; }

        public Nullable<decimal> valor_collect { get; set; }

        public Nullable<decimal> basis { get; set; }

        public Nullable<decimal> rate { get; set; }

        public string right_hand_note { get; set; }

        public Single valor_sobreventa { get; set; }

        public bool local { get; set; }

        public Nullable<long> id_servicio { get; set; }

        public Nullable<long> factura_id { get; set; }

        public Nullable<long> tipo_conta { get; set; }

        public Nullable<int> tipo_cobro { get; set; }

        public Nullable<int> tipo_documento { get; set; }

        public bool activo { get; set; }

        public Nullable<int> id_usuario { get; set; }

        public DateTime fecha_registro { get; set; }

        public Nullable<decimal> valor { get; set; }
    }
}
