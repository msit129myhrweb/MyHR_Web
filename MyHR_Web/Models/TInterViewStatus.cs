using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TInterViewStatus
    {
        public TInterViewStatus()
        {
            TInterViews = new HashSet<TInterView>();
        }

        public int InterViewStatusId { get; set; }
        public string InterViewStatus { get; set; }

        public virtual ICollection<TInterView> TInterViews { get; set; }
    }
}
