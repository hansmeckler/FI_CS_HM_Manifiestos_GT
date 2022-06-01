using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("container_type", Schema = "public")]
    public class container_type
    {
        [Key]
        [Column("id_container_type")]
        public int id_container_type { get; set; }
        [Column("short_name")]
        public string short_name { get; set; }
        [Column("description")]
        public string description { get; set; }
        [Column("iso")]
        public string iso { get; set; }
        [Column("mt")]
        public int mt { get; set; }
        [Column("id_container_class")]
        public int id_container_class { get; set; }
    }
}
