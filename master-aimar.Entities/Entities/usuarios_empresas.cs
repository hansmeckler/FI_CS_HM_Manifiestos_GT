using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("usuarios_empresas", Schema = "public")]
    public class usuarios_empresas
    {
        [Key]
        public int id_usuario { get; set; }

        public string pw_name { get; set; }

        public string pw_passwd { get; set; }

        public DateTime pw_passwd_fecha { get; set; }

        public int pw_passwd_dias { get; set; }

        public string dominio { get; set; }

        public int pw_activo { get; set; }

        public string pw_gecos { get; set; }

    }

}
