using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("aausers", Schema = "public")]
    public class cuscar_users
    {
        [Key]
        public int userid { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public bool admin { get; set; }

        public bool aereo { get; set; }

        public bool marino { get; set; }

        public long id_usuario { get; set; }
    }
}
