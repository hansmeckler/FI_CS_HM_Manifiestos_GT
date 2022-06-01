using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("usuarios_permisos_manifiestos", Schema = "public")]
    public class usuarios_permisos_manifiestos
    {
        [Key]
        public int id { get; set; }

        public Nullable<long> id_usuario { get; set; }

        public Nullable<int> id_empresa { get; set; }

        public Nullable<int> id_menu { get; set; }

        public Nullable<DateTime> fecha_creado { get; set; }
    }
}
