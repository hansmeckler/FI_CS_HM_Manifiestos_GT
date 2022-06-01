using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("aereo_temp", Schema = "public")]
    public class aereo_temp
    {
        [Key]
        public long id { get; set; }

        public Nullable<int> anio { get; set; }

        public Nullable<bool> import_export { get; set; }

        public Nullable<bool> cif { get; set; }

        public string region { get; set; }

        public string nombre_pais { get; set; }

        public Nullable<decimal> volumen { get; set; }
    }
}
