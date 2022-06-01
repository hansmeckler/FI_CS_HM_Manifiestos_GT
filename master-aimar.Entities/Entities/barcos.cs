using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("barcos", Schema = "public")]
    public class barcos
    {
        [Key]
        [Column("id_barco")]
        public int id_barco { get; set; }
        [Column("barco")]
        public string barco { get; set; }
        [Column("nombre")]
        public string nombre { get; set; }
        [Column("pais")]
        public string pais { get; set; }
        [Column("imo")]
        public string imo { get; set; }
        [Column("activo")]
        public bool activo { get; set; }
    }
}
