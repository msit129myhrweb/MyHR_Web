using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLostPropertyCheckStatus
    {
        public TLostPropertyCheckStatus()
        {
            TLosts = new HashSet<TLost>();
        }

        public byte CLostCheckStatusId { get; set; }
        public string CLostCheckStatus { get; set; }

        public virtual ICollection<TLost> TLosts { get; set; }
    }
}
