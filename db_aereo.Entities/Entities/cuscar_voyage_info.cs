using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace db_aereo.Entities.Entities
{
    [Table("cuscar_voyage_info")]
    public class cuscar_voyage_info
    {
        [Key]
        public int cuscar_voyage_id { get; set; }
        public string no_viaje { get; set; }
        public string naviera { get; set; }
        public string vapor { get; set; }
        public string fecha_arribo { get; set; }
        public string cuscar { get; set; }
        public string manifest { get; set; }
        public string original { get; set; }
        public Nullable<int> viaje_id { get; set; }
        public Nullable<int> id_naviera { get; set; }
        public Nullable<int> id_puerto_origen { get; set; }
        public Nullable<int> id_puerto_desembarque { get; set; }
        public string puerto_origen { get; set; }
        public string puerto_desembarque { get; set; }
        public string tipo { get; set; }
        public string fecha_salida { get; set; }
        public Nullable<int> mtype { get; set; }
        public Nullable<int> mfunction { get; set; }
        public Nullable<int> operation { get; set; }
        public string funcsend { get; set; }
        public Nullable<int> test { get; set; }
        public Nullable<System.DateTime> cuscardt { get; set; }
        public string countries { get; set; }
        public int id_status { get; set; }
    }
}
