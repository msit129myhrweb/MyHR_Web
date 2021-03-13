using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLeave
    {
        public TLeave()
        {
            TLeaveApplications = new HashSet<TLeaveApplication>();
        }

        public int CLeaveId { get; set; }
        public string CLeaveCategory { get; set; }

        public virtual ICollection<TLeaveApplication> TLeaveApplications { get; set; }
    }
}
