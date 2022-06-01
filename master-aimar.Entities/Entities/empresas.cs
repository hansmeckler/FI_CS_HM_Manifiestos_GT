using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("empresas", Schema = "public")]
    public class empresas
    {
        [Key]
        public int id_empresa { get; set; }

        public string pais_iso { get; set; }

        public string nombre_empresa { get; set; }

        public string nombre_pais { get; set; }

        public bool activo { get; set; }
    }
}
