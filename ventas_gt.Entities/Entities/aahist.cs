using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ventas_gt.Entities.Entities
{
    [Table("aahist", Schema = "public")]
    public class aahist
    {
        public int id { get; set; }

        public Nullable<int> mn_id { get; set; }

        public string nm_table { get; set; }

        public string chgfield { get; set; }

        public string chgfrom { get; set; }

        public string chgto { get; set; }

        public string id_field { get; set; }

        public Nullable<int> id_value { get; set; }

        public string myuser { get; set; }

        public Nullable<DateTime> update { get; set; }
    }
}
