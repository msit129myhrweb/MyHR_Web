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

        public int CPropertyCheckStatusId { get; set; }
        public string CPropertyCheckStatus { get; set; }

        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
    }
}
