using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TCheckStatus
    {
        public TCheckStatus()
        {
            TLeaveApplications = new HashSet<TLeaveApplication>();
            TTravelExpenseApplications = new HashSet<TTravelExpenseApplication>();
        }

        public int CCheckStatusId { get; set; }
        public string CCheckStatus { get; set; }

        public virtual ICollection<TLeaveApplication> TLeaveApplications { get; set; }
        public virtual ICollection<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
    }
}
