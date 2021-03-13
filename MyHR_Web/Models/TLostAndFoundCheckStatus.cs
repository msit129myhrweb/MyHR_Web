using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLostAndFoundCheckStatus
    {
        public TLostAndFoundCheckStatus()
        {
            TLostAndFounds = new HashSet<TLostAndFound>();
        }

        public int CcPropertyCheckStatusId { get; set; }
        public string CcPropertyCheckStatus { get; set; }

        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
    }
}
