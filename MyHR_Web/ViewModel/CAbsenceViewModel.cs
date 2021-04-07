using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CAbsenceViewModel
    {
        private TAbsence iv_absence = null;
        private TUser iv_User = null;
        //private TUserDepartment iv_Dep = null;

        public TAbsence absence { get { return iv_absence; } }
        public TUser User { get { return iv_User; } }
        //public TUserDepartment Dep { get { return iv_Dep; } }

        public CAbsenceViewModel(TAbsence a, TUser u/*, TUserDepartment d*/)
        {
            iv_absence = a;
            iv_User = u;
            //iv_Dep = d;
        }
        public CAbsenceViewModel()
        {
            iv_absence = new TAbsence();
            iv_User = new TUser();
            //iv_Dep = new TUserDepartment();
        }
        public int CApplyNumber 
        {
            get { return iv_absence.CApplyNumber; }
            set { iv_absence.CApplyNumber = value; } 
        }
        [DisplayName("員工編號")]
        public int CEmployeeId 
        {
            get { return iv_absence.CEmployeeId; }
            set { iv_absence.CEmployeeId = value; }
        }
        [DisplayName("日期")]

        public DateTime? CDate
        {
            get { return iv_absence.CDate; }
            set { iv_absence.CDate = value; }
        }
        [DisplayName("上班")]
        public TimeSpan? COn 
        {
            get { return iv_absence.COn; }
            set { iv_absence.COn = value; }
        }
        [DisplayName("下班")]
        public TimeSpan? COff 
        {
            get { return iv_absence.COff; }
            set { iv_absence.COff = value; } 
        }
        [DisplayName("狀態")]
        public string CStatus
        {
            get {return iv_absence.CStatus; }
            set {iv_absence.CStatus=value; } 
        }
        [DisplayName("員工姓名")]
        public string employeeName { get { return iv_User.CEmployeeName; } set { iv_User.CEmployeeName = value; } }
        //[DisplayName("部門名稱")]
        //public string CDepartment
        //{
        //    get {
        //        CDepartment = ((eDepartment)iv_Dep.CDepartmentId).ToString();
        //        return CDepartment; 
        //    }
        //    set { CDepartment = value; }
        //}
    }
}
