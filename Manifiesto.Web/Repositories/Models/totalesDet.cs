using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manifiesto.Data.Models
{
    public class totalesDet
    {
        public Nullable<decimal> peso { get; set; }

        public Nullable<decimal> volumen { get; set; }

        public Nullable<long> no_piezas { get; set; }

        public Nullable<long> count { get; set; }
    }

    public class totalesDet2
    {
        public Nullable<int> count { get; set; }

        public Nullable<decimal> peso { get; set; }

        public Nullable<decimal> volumen { get; set; }

        public Nullable<long> no_piezas { get; set; }
    }

}
