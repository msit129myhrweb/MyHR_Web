using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class TLeaveApplicationViewModel
    {

        dbMyCompanyContext MyHR = new dbMyCompanyContext();        private TLeaveApplication iv_Leave = null;
        private TUserDepartment iv_Department = null;
        private TUser iv_User = null;
        public TUser user { get { return iv_User; } }

        public TLeaveApplicationViewModel(TLeaveApplication p, TUser u)
        {
            iv_Leave = p;
            iv_User = u;
        }
        public TLeaveApplicationViewModel(TLeaveApplication p,TUserDepartment d, TUser u)        {
            iv_Leave = p;
            iv_Department = d;
            iv_User = u;
        }
        //------------------------------------------------------------

        public TLeaveApplicationViewModel()
        {
            iv_Leave = new TLeaveApplication();
 			iv_User = new TUser();
			iv_Department = new TUserDepartment();
        }
       
        [DisplayName("部門名稱")]
        public string CDepartmentName { get { return iv_Department.CDepartment; } set { iv_Department.CDepartment = value; } }

        [DisplayName("申請單號")]
        public int CApplyNumber { get { return iv_Leave.CApplyNumber; } set { iv_Leave.CApplyNumber = value; } }

        [DisplayName("員工姓名")] //取得ID值，轉換成員工名稱
        public string CEmployeeName { get {
                iv_User.CEmployeeName = MyHR.TUsers.Where(n => n.CEmployeeId == CEmployeeId).Select(n => n.CEmployeeName).FirstOrDefault();
                return iv_User.CEmployeeName; } set { iv_User.CEmployeeName = value; } }
        [DisplayName("員工編號")]
        public int CEmployeeId { get { return iv_Leave.CEmployeeId; } set { iv_Leave.CEmployeeId = value; } }
        

        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]

        [DisplayName("部門名稱")] //新增時，取得部門名稱，轉換成部門ID，存進資料庫
        public int CDepartmentId { get {iv_Leave.CDepartmentId = MyHR.TUserDepartments.Where(n => n.CDepartment == CDepartmentName).Select(n => n.CDepartmentId).FirstOrDefault();
                return iv_Leave.CDepartmentId; } set { iv_Leave.CDepartmentId = value; } }

        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("申請日")]
        public string CApplyDate { get { return DateTime.Parse(iv_Leave.CApplyDate).ToString("yyyy/MM/dd"); } set { iv_Leave.CApplyDate = value; } }
        [DisplayName("申請類別")]
        [Required(ErrorMessage = "必填欄位")]
        public int CLeaveCategory { get { return iv_Leave.CLeaveCategory; } set { iv_Leave.CLeaveCategory = value; } }

        [DisplayName("請假起始日")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt h:mm}")]
        public string CLeaveStartTime { get { return DateTime.Parse(iv_Leave.CLeaveStartTime).ToString("yyyy/MM/dd - HH:mm"); } set { iv_Leave.CLeaveStartTime = value; } }
        [DisplayName("請假結束日")]
        [Required(ErrorMessage = "必填欄位")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd tt h:mm}")]
        public string CLeaveEndTime { get { return DateTime.Parse(iv_Leave.CLeaveEndTime).ToString("yyyy/MM/dd - HH:mm"); } set { iv_Leave.CLeaveEndTime = value; } }
        [DisplayName("請假原由")]
        [Required(ErrorMessage = "必填欄位")]

        public string CReason { get { return iv_Leave.CReason; } set { iv_Leave.CReason = value; } }
        [DisplayName("狀態")]
        public int CCheckStatus { get { return iv_Leave.CCheckStatus; } set { iv_Leave.CCheckStatus = value; } }
        
        [DisplayName("請假時數")]
        public int? CLeaveHours { get { return iv_Leave.CLeaveHours; } set { iv_Leave.CLeaveHours = value; } }

        //public virtual TCheckStatus CCheckStatusNavigation { get { return iv_Leave.CCheckStatusNavigation; } set { iv_Leave.CCheckStatusNavigation = value; } }
        //public virtual TUser CEmployee { get { return iv_Leave.CEmployee; } set { iv_Leave.CEmployee = value; } }
        //public virtual TLeave CLeaveCategoryNavigation { get { return iv_Leave.CLeaveCategoryNavigation; } set { iv_Leave.CLeaveCategoryNavigation = value; } }
        [DisplayName("員工姓名")]

        public string employeeName { get{return iv_User.CEmployeeName ; } set{iv_User.CEmployeeName=value ; }}

    }


}

