using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLostAndFound
    {
        public int CPropertyId { get; set; }
        public int CEmployeeId { get; set; }
        public int CDeparmentId { get; set; }
        public string CPhone { get; set; }
        public int CPropertySubjectId { get; set; }
        public int CPropertyCategoryId { get; set; }
        public byte[] CPropertyPhoto { get; set; }
        public string CProperty { get; set; }
        public DateTime? CLostAndFoundDate { get; set; }
        public string CLostAndFoundSpace { get; set; }
        public string CtPropertyDescription { get; set; }
        public int CPropertyCheckStatusId { get; set; }

        public virtual TUserDepartment CDeparment { get; set; }
        public virtual TUser CEmployee { get; set; }
        public virtual TLostAndFoundCategory CPropertyCategory { get; set; }
        public virtual TLostAndFoundCheckStatus CPropertyCheckStatus { get; set; }
        public virtual TLostAndFoundSubject CPropertySubject { get; set; }
    }
}
