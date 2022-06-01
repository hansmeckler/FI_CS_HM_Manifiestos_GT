using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("region", Schema = "public")]
    public class region
    {
        [Key]
        public long id_region { get; set; }

        public string descripcion { get; set; }
    }
}
