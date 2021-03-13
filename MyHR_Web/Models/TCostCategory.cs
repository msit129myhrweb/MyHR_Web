using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TCostCategory
    {
        public TCostCategory()
        {
            TApplyDetails = new HashSet<TApplyDetail>();
        }

        public int CCostId { get; set; }
        public string CCostCategory { get; set; }

        public virtual ICollection<TApplyDetail> TApplyDetails { get; set; }
    }
}
