using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("aduanas", Schema = "public")]
    public class aduanas
    {
        [Key]
        public long id_aduana { get; set; }

        public string gln { get; set; }

        public string nombre_aduana { get; set; }

    }

}
