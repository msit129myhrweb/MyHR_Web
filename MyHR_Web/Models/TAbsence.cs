using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TAbsence
    {
        public int CApplyNumber { get; set; }
        public int CEmployeeId { get; set; }
        public DateTime? CDate { get; set; }
        public TimeSpan? COn { get; set; }
        public TimeSpan? COff { get; set; }
        public string CStatus { get; set; }

        public virtual TUser CEmployee { get; set; }
    }
}
