using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("navieras", Schema = "public")]
    public class navieras
    {
        [Key]
        public long id_naviera { get; set; }

        public string nombre { get; set; }

        public bool activo { get; set; }
     
        public Nullable<int> tiporegimen { get; set; }
   
        public Nullable<int> dias { get; set; }

        public string nit { get; set; }

        public string nit2 { get; set; }

        public string id_pais { get; set; }
    }
}
