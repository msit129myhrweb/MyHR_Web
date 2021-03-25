using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TTravelExpenseApplication
    {
        public int CApplyNumber { get; set; }
        public int CDepartmentId { get; set; }
        public int CEmployeeId { get; set; }
        public string CReason { get; set; }
        public DateTime? CApplyDate { get; set; }
        public DateTime? CTravelStartTime { get; set; }
        public DateTime? CTravelEndTime { get; set; }
        public decimal CAmont { get; set; }
        public int CCheckStatus { get; set; }

        public virtual TCheckStatus CCheckStatusNavigation { get; set; }
        public virtual TUserDepartment CDepartment { get; set; }
        public virtual TUser CEmployee { get; set; }
    }
}
