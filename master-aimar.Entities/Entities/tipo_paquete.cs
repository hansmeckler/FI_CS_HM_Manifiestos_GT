using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("tipo_paquete", Schema = "public")]
    public class tipo_paquete
    {
        [Key]
        [Column("tipo_id")]
        public long tipo_id { get; set; }
        [Column("tipo")]
        public string tipo { get; set; }
        [Column("activo")]
        public bool activo { get; set; }
        [Column("iso")]
        public string iso { get; set; }
    }
}
