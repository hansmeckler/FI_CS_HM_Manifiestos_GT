using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace db_aereo.Entities.Entities
{
    [Table("Airports")]
    public class Airports
    {
        [Key]
        public int AirportID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<double> CreatedTime { get; set; }
        public double Expired { get; set; }
        public string AirportCode { get; set; }
        public string Country { get; set; }
    }
}
