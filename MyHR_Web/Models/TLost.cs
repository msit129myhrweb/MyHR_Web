using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLost
    {
        public int CLostId { get; set; }
        public int CEmployeeId { get; set; }
        public int CDeparment { get; set; }
        public string CPhone { get; set; }
        public string CLostSubject { get; set; }
        public string CLostCategory { get; set; }
        public byte[] CLostPropertyPhoto { get; set; }
        public string CLostProperty { get; set; }
        public DateTime CLostDate { get; set; }
        public string CLostSpace { get; set; }
        public string CLostPropertyDescription { get; set; }
        public byte CLostCheckStatus { get; set; }

        public virtual TUser CEmployee { get; set; }
        public virtual TLostPropertyCheckStatus CLostCheckStatusNavigation { get; set; }
    }
}
