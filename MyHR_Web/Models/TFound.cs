using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TFound
    {
        public int CFoundId { get; set; }
        public int CEmployeeId { get; set; }
        public int CDeparment { get; set; }
        public string CPhone { get; set; }
        public string CFoundSubject { get; set; }
        public string CFoundCategory { get; set; }
        public byte[] CFoundPropertyPhoto { get; set; }
        public string CFoundProperty { get; set; }
        public DateTime CFoundDate { get; set; }
        public string CFoundSpace { get; set; }
        public string CFoundPropertyDescription { get; set; }
        public byte CFoundCheckStatusId { get; set; }

        public virtual TUserDepartment CDeparmentNavigation { get; set; }
        public virtual TUser CEmployee { get; set; }
        public virtual TFoundCheckStatus CFoundCheckStatus { get; set; }
    }
}
