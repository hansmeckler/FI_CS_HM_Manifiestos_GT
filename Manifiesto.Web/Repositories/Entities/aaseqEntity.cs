using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manifiesto.Data.Entities
{
    public class aaseqEntity
    {
        public Nullable<int> nexttransmission { get; set; }

        public Nullable<int> nextmessage { get; set; }

        public Nullable<int> nextmanifest { get; set; }

        public Nullable<int> nextarchive { get; set; }

        public Nullable<int> nextatransmission { get; set; }

        public Nullable<int> nextamessage { get; set; }

        public Nullable<int> nextamanifest { get; set; }

        public string firma { get; set; }

        public string keyid { get; set; }

        public Nullable<int> nextctransmission { get; set; }

        public Nullable<int> nextcmessage { get; set; }

        public Nullable<int> nextcmanifest { get; set; }

        public Nullable<int> nextcarchive { get; set; }
    }
}
