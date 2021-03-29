using Microsoft.AspNetCore.Http;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class TLeaveApplicationEditViewModel
    {


        private TLeaveApplication iv_Leave = null;
        public TLeaveApplication Leave { get { return iv_Leave; } }

        public TLeaveApplicationEditViewModel(TLeaveApplication p)
        {
            iv_Leave = p;
        }

        public TLeaveApplicationEditViewModel()
        {
            iv_Leave = new TLeaveApplication();
        }


        public string cDepartmentName = null;
        [DisplayName("部門名稱")] //自己新增
                
        public string CDepartmentName
        {
            get
            {
                cDepartmentName = ((eDepartmentEnum)iv_Leave.CDepartmentId).ToString();
                return cDepartmentName;
            }
            set { CDepartmentName = value; }
        }

        public int CApplyNumber { get { return iv_Leave.CApplyNumber; } set { iv_Leave.CApplyNumber = value; } }
        
        [DisplayName("部門編號")]

        public int CDepartmentId
        {
            get
            {
                iv_Leave.CDepartmentId = (int)Enum.Parse(typeof(eDepartmentEnum), CDepartmentName);
                return iv_Leave.CDepartmentId;

            }
            set { iv_Leave.CDepartmentId = value; }
        }

        [DisplayName("員工名稱")]//自己新增
        public string CUserName { get; set; }


        [DisplayName("員工編號")]

        public int CEmployeeId
        {
            get{ return iv_Leave.CEmployeeId;} set { iv_Leave.CEmployeeId = value; }

        }


        [DisplayName("申請日")]
       
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]

        public string CApplyDate { get { return iv_Leave.CApplyDate; } set { iv_Leave.CApplyDate = value; } }


        
        
        public int CLeaveCategory { 
            get {
                iv_Leave.CLeaveCategory = (int)Enum.Parse(typeof(eLeaveCategory), CLeaveCategoryName);
                return iv_Leave.CLeaveCategory;
            }set { iv_Leave.CLeaveCategory = value; } }

        [DisplayName("申請類別")]
        public string CLeaveCategoryName { get; set; }   //自己新增





        [DisplayName("請假起始日")]
        
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string CLeaveStartTime { get { return iv_Leave.CLeaveStartTime; } set { iv_Leave.CLeaveStartTime = value; } }
        [DisplayName("請假結束日")]
        
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string CLeaveEndTime { get { return iv_Leave.CLeaveEndTime; } set { iv_Leave.CLeaveEndTime = value; } }
        
        [Required]
        [DisplayName("請假原由")]
        

        public string CReason { get { return iv_Leave.CReason; } set { iv_Leave.CReason = value; } }
        [DisplayName("狀態")]
        public int CCheckStatus { get { return iv_Leave.CCheckStatus; } set { iv_Leave.CCheckStatus = value; } }

        //public virtual TCheckStatus CCheckStatusNavigation { get { return iv_Leave.CCheckStatusNavigation; } set { iv_Leave.CCheckStatusNavigation = value; } }
        public virtual TUser CEmployee { get { return iv_Leave.CEmployee; } set { iv_Leave.CEmployee = value; } }
        public virtual TLeave CLeaveCategoryNavigation { get { return iv_Leave.CLeaveCategoryNavigation; } set { iv_Leave.CLeaveCategoryNavigation = value; } }
    }


}


