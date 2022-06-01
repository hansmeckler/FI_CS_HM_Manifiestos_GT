using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("direcciones", Schema = "public")]
    public class direcciones
    {
        [Key]
        public long id_direccion { get; set; }

        public Nullable<long> id_nivel_geografico { get; set; }

        public string direccion_completa { get; set; }

        public Nullable<long> id_cliente { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public string name { get; set; }

        public string phone_number { get; set; }

        public string group { get; set; }

        public string url { get; set; }

        public string image { get; set; }

        public Nullable<decimal> lat { get; set; }

        public Nullable<decimal> Ing { get; set; }

        public string email { get; set; }

        public Nullable<int> id_tipo_direccion { get; set; }

        public bool boletines { get; set; }

        public bool activo { get; set; }
    }
}
