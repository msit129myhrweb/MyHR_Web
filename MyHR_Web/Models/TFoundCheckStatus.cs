using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TFoundCheckStatus
    {
        public TFoundCheckStatus()
        {
            TFounds = new HashSet<TFound>();
        }

        public byte CFoundCheckStatusId { get; set; }
        public string CFoundCheckStatus { get; set; }

        public virtual ICollection<TFound> TFounds { get; set; }
    }
}
