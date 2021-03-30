using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TAbsence
    {
        public int CApplyNumber { get; set; }
        public int CEmployeeId { get; set; }
        public DateTime? COn { get; set; }
        public DateTime? COff { get; set; }

        public virtual TUser CEmployee { get; set; }
    }
}
