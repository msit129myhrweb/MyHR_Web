using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TUser
    {
        public TUser()
        {
            TAbsences = new HashSet<TAbsence>();
            TInterViews = new HashSet<TInterView>();
            TLeaveApplications = new HashSet<TLeaveApplication>();
            TLostAndFounds = new HashSet<TLostAndFound>();
            TRepairs = new HashSet<TRepair>();
            TTravelExpenseApplications = new HashSet<TTravelExpenseApplication>();
            TWuChaOrders = new HashSet<TWuChaOrder>();
        }
        [DisplayName("員工編號")]
        public int CEmployeeId { get; set; }
        [DisplayName("姓名")]
        public string CEmployeeName { get; set; }
        [DisplayName("英文姓名")]
        public string CEmployeeEnglishName { get; set; }
        [DisplayName("密碼")]
        public string CPassWord { get; set; }
        [DisplayName("到職日")]
        public DateTime COnBoardDay { get; set; }
        [DisplayName("離職日")]
        public DateTime? CByeByeDay { get; set; }
        [DisplayName("性別")]
        public string CGender { get; set; }
        [DisplayName("Email")]
        public string CEmail { get; set; }
        [DisplayName("地址")]
        public string CAddress { get; set; }
        [DisplayName("部門")]
        public int CDepartmentId { get; set; }
        [DisplayName("職務")]
        public int CJobTitleId { get; set; }
        [DisplayName("主管")]
        public int? CSupervisor { get; set; }
        [DisplayName("生日")]
        public DateTime CBirthday { get; set; }
        [DisplayName("電話")]
        public string CPhone { get; set; }
        [DisplayName("照片")]
        public byte[] CPhoto { get; set; }
        [DisplayName("緊急聯絡人")]
        public string CEmergencyPerson { get; set; }
        [DisplayName("緊急連絡電話")]
        public string CEmergencyContact { get; set; }
        [DisplayName("到職狀態")]
        public int COnBoardStatusId { get; set; }
        [DisplayName("帳號狀態")]
        public byte CAccountEnable { get; set; }

        public virtual TUserOnBoardStatus COnBoardStatus { get; set; }
        public virtual ICollection<TAbsence> TAbsences { get; set; }
        public virtual ICollection<TInterView> TInterViews { get; set; }
        public virtual ICollection<TLeaveApplication> TLeaveApplications { get; set; }
        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
        public virtual ICollection<TRepair> TRepairs { get; set; }
        public virtual ICollection<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
        public virtual ICollection<TWuChaOrder> TWuChaOrders { get; set; }


    }
}
