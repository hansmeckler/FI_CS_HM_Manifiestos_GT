using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("usuarios_x_empresa", Schema = "public")]
    public class usuarios_x_empresa
    {
        [Key]
        public long id { get; set; }

        public Nullable<long> id_usuario { get; set; }

        public int id_empresa { get; set; }

        public Nullable<DateTime> fecha_creado { get; set; }

        public Nullable<int> id_menu { get; set; }
  
    }
}
