using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{

    [Table("unlocode", Schema = "public")]
    public class unlocode
    {
        [Key]
        public long unlocode_id { get; set; }

        public string codigo { get; set; }

        public string nombre { get; set; }

        public string iso_3166 { get; set; }

        public string locode { get; set; }

        public string pais { get; set; }

        public string depto { get; set; }

    }
}
