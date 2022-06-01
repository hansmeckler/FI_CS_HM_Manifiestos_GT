using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("clientes", Schema = "public")]
    public class clientes
    {
        [Key]
        [Column("id_cliente")]
        public long id_cliente { get; set; }
        [Column("codigo_tributario")]
        public string codigo_tributario { get; set; }
        [Column("nombre_cliente")]
        public string nombre_cliente { get; set; }
        [Column("nombre_facturar")]
        public string nombre_facturar { get; set; }
        [Column("id_vendedor")]
        public Nullable<long> id_vendedor { get; set; }
        [Column("id_tipo_cliente")]
        public Nullable<int> id_tipo_cliente { get; set; }
        [Column("id_grupo")]
        public Nullable<int> id_grupo { get; set; }
    }
}
