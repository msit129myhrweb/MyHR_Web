using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TApplyDetail
    {
        public int CApplyNumber { get; set; }
        public int CCostId { get; set; }
        public decimal CAmont { get; set; }

        public virtual TCostCategory CCost { get; set; }
    }
}
