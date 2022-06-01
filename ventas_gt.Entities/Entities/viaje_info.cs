using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ventas_gt.Entities.Entities
{
    [Table("aamn", Schema = "public")]
    public class viaje_info
    {
        [Key]
        [Column("id")]
        public long id { get; set; }

        [Column("viaje_id")]
        public int viaje_id { get; set; }

        [Column("no_viaje")]
        public string no_viaje { get; set; }

        [Column("vapor")]
        public string vapor { get; set; }

        [Column("id_naviera")]
        public int id_naviera { get; set; }

        [Column("id_puerto_origen")]
        public int id_puerto_origen { get; set; }

        [Column("id_puerto_desembarque")]
        public int id_puerto_desembarque { get; set; }

        [Column("agente")]
        public Nullable<int> agente { get; set; }

        [Column("user_id")]
        public Nullable<int> user_id { get; set; }

        [Column("naviera")]
        public string naviera { get; set; }

        [Column("puerto_origen")]
        public string puerto_origen { get; set; }

        [Column("puerto_desembarque")]
        public string puerto_desembarque { get; set; }

        [Column("tipo")]
        public string tipo { get; set; }

        [Column("fecha_salida")]
        public Nullable<DateTime> fecha_salida { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("fecha_arribo")]
        public Nullable<DateTime> fecha_arribo { get; set; }

        [Column("eta")]
        public Nullable<DateTime> eta { get; set; }

        [Column("etd")]
        public Nullable<DateTime> etd { get; set; }

    }
}
