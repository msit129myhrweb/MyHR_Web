using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TInterViewProcess
    {
        public TInterViewProcess()
        {
            TInterViews = new HashSet<TInterView>();
        }

        public int CInterViewProcessId { get; set; }
        public string CInterViewProcess { get; set; }

        public virtual ICollection<TInterView> TInterViews { get; set; }
    }
}
