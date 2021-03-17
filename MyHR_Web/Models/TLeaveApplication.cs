using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLeaveApplication
    {
        public int CApplyNumber { get; set; }
        public int CDepartmentId { get; set; }
        public int CEmployeeId { get; set; }
        public DateTime CApplyDate { get; set; }
        public int CLeaveCategory { get; set; }
        public DateTime CLeaveStartTime { get; set; }
        public DateTime CLeaveEndTime { get; set; }
        public string CReason { get; set; }
        public int CCheckStatus { get; set; }

        public virtual TUserDepartment CDepartment { get; set; }
        public virtual TUser CEmployee { get; set; }
        public virtual TLeave CLeaveCategoryNavigation { get; set; }
    }
}
