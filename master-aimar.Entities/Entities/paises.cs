using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("paises", Schema = "public")]
    public class paises
    {
        [Key]
        public string codigo { get; set; }

        public string descripcion { get; set; }

        public Nullable<long> id_region { get; set; }
    }
}
