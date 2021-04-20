using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TUser
    {
        public TUser()
        {
            TAbsences = new HashSet<TAbsence>();
            TEvents = new HashSet<TEvent>();
            TInterViewProcesses = new HashSet<TInterViewProcess>();
            TInterViews = new HashSet<TInterView>();
            TLeaveApplications = new HashSet<TLeaveApplication>();
            TLostAndFounds = new HashSet<TLostAndFound>();
            TRepairs = new HashSet<TRepair>();
            TTravelExpenseApplications = new HashSet<TTravelExpenseApplication>();
            TWuChaOrders = new HashSet<TWuChaOrder>();
        }

        public int CEmployeeId { get; set; }
        public string CEmployeeName { get; set; }
        public string CEmployeeEnglishName { get; set; }
        public string CPassWord { get; set; }
        public DateTime COnBoardDay { get; set; }
        public DateTime? CByeByeDay { get; set; }
        public string CGender { get; set; }
        public string CEmail { get; set; }
        public string CAddress { get; set; }
        public int CDepartmentId { get; set; }
        public int CJobTitleId { get; set; }
        public int? CSupervisor { get; set; }
        public DateTime? CBirthday { get; set; }
        public string CPhone { get; set; }
        public byte[] CPhoto { get; set; }
        public string CEmergencyPerson { get; set; }
        public string CEmergencyContact { get; set; }
        public int COnBoardStatusId { get; set; }
        public byte? CAccountEnable { get; set; }

        public virtual TUserDepartment CDepartment { get; set; }
        public virtual TUserJobTitle CJobTitle { get; set; }
        public virtual TUserOnBoardStatus COnBoardStatus { get; set; }
        public virtual ICollection<TAbsence> TAbsences { get; set; }
        public virtual ICollection<TEvent> TEvents { get; set; }
        public virtual ICollection<TInterViewProcess> TInterViewProcesses { get; set; }
        public virtual ICollection<TInterView> TInterViews { get; set; }
        public virtual ICollection<TLeaveApplication> TLeaveApplications { get; set; }
        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
        public virtual ICollection<TRepair> TRepairs { get; set; }
        public virtual ICollection<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
        public virtual ICollection<TWuChaOrder> TWuChaOrders { get; set; }
    }
}
