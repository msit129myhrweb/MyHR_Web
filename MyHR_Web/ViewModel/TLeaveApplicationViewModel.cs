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

        dbMyCompanyContext MyHR = new dbMyCompanyContext();
        private TLeaveApplication iv_Leave = null;
        private TUserDepartment iv_Department = null;
        private TUser iv_User = null;
        public TUser user { get { return iv_User; } }

        public TLeaveApplicationViewModel(TLeaveApplication p, TUser u)
        {
            iv_Leave = p;
            iv_User = u;
        } 
        public TLeaveApplicationViewModel(TLeaveApplication p,TUserDepartment d, TUser u)
        {
            iv_Leave = p;
            iv_Department = d;
            iv_User = u;
        }
        public TLeaveApplicationViewModel()
        {
            iv_Leave = new TLeaveApplication();
 			iv_User = new TUser();
			iv_Department = new TUserDepartment();
        }
       
        [DisplayName("部門名稱")]
        public string CDepartmentName { get { return iv_Department.CDepartment; } set { iv_Department.CDepartment = value; } }
        
       
        public int CApplyNumber { get { return iv_Leave.CApplyNumber; } set { iv_Leave.CApplyNumber = value; } }

        [DisplayName("員工名稱")] //取得ID值，轉換成員工名稱
        public string CEmployeeName { get {
                iv_User.CEmployeeName = MyHR.TUsers.Where(n => n.CEmployeeId == CEmployeeId).Select(n => n.CEmployeeName).FirstOrDefault();
                return iv_User.CEmployeeName; } set { iv_User.CEmployeeName = value; } }
        [DisplayName("員工名稱")]
        public int CEmployeeId { get { return iv_Leave.CEmployeeId; } set { iv_Leave.CEmployeeId = value; } }
        


        public int CApplyNumber { get {return iv_Leave.CApplyNumber; } set {iv_Leave.CApplyNumber=value; } }
    
        [DisplayName("部門名稱")]
        public int CDepartmentId { get { return iv_Leave.CDepartmentId; } set { iv_Leave.CDepartmentId = value; } }
        [DisplayName("員工編號")]
        public int CEmployeeId { get { return iv_Leave.CEmployeeId; } set { iv_Leave.CEmployeeId = value; } }
        [DisplayName("申請日")]
        public string CApplyDate { get { return iv_Leave.CApplyDate; } set { iv_Leave.CApplyDate = value; } }        [DisplayName("申請類別")]
        [Required(ErrorMessage = "必填欄位")]
        public int CLeaveCategory { get { return iv_Leave.CLeaveCategory; } set { iv_Leave.CLeaveCategory = value; } }
        [DisplayName("請假起始日")]
        public string CLeaveStartTime { get { return iv_Leave.CLeaveStartTime; } set { iv_Leave.CLeaveStartTime = value; } }
        [DisplayName("請假結束日")]
        public string CLeaveEndTime { get { return iv_Leave.CLeaveEndTime; } set { iv_Leave.CLeaveEndTime = value; } }
        [DisplayName("請假原由")]
        [Required(ErrorMessage = "必填欄位")]

        public string CReason { get { return iv_Leave.CReason; } set { iv_Leave.CReason = value; } }
        [DisplayName("狀態")]
        public int CCheckStatus { get { return iv_Leave.CCheckStatus; } set { iv_Leave.CCheckStatus = value; } }

        //public virtual TCheckStatus CCheckStatusNavigation { get { return iv_Leave.CCheckStatusNavigation; } set { iv_Leave.CCheckStatusNavigation = value; } }
        //public virtual TUser CEmployee { get { return iv_Leave.CEmployee; } set { iv_Leave.CEmployee = value; } }
        //public virtual TLeave CLeaveCategoryNavigation { get { return iv_Leave.CLeaveCategoryNavigation; } set { iv_Leave.CLeaveCategoryNavigation = value; } }

        public string employeeName { get{return iv_User.CEmployeeName ; } set{iv_User.CEmployeeName=value ; }}

    }


}

