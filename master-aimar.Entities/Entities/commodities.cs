using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace master_aimar.Entities.Entities
{
    [Table("commodities", Schema = "public")]
    public class commodities
    {
        [Key]
        public long commodityid { get; set; }

        public string namees { get; set; }

        public string nameen { get; set; }

        public Nullable<decimal> typeval { get; set; }

        public decimal createdtime { get; set; }

        public decimal expired { get; set; }

        public string commoditycode { get; set; }

        public Nullable<DateTime> createddate { get; set; }

        public string arancel_gt { get; set; }

        public string arancel_sv { get; set; }

        public string arancel_hn { get; set; }

        public string arancel_ni { get; set; }

        public string arancel_cr { get; set; }

        public string arancel_pa { get; set; }

        public string arancel_bz { get; set; }

        public Nullable<decimal> reqauth { get; set; }
    }
}
