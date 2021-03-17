using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TCheckStatus
    {
        public TCheckStatus()
        {
            TTravelExpenseApplications = new HashSet<TTravelExpenseApplication>();
        }

        public int CCheckStatusId { get; set; }
        public string CCheckStatus { get; set; }

        public virtual ICollection<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
    }
}
