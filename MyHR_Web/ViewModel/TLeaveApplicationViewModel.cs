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


        private TLeaveApplication iv_Leave = null;
        public TLeaveApplication Leave { get { return iv_Leave; } }

        public TLeaveApplicationViewModel(TLeaveApplication p)
        {
            iv_Leave = p;
        }

        public TLeaveApplicationViewModel()
        {
            iv_Leave = new TLeaveApplication();
        }



        public int CApplyNumber { get; set; }


        [Required(ErrorMessage = "必填欄位")]
        public int CDepartmentId { get; set; }

        [DisplayName("部門名稱")]
        public string CDepartmentName { get; set; }

        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("員工編號")]
        public int CEmployeeId { get; set; }

        [DisplayName("申請日")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CApplyDate { get; set; }

        [DisplayName("申請類別")]
        [Required(ErrorMessage = "必填欄位")]
        public string CLeaveCategory { get; set; }

        public int CLeaveCategoryId { get; set; }





        [DisplayName("請假起始日")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CLeaveStartTime { get; set; }

        [DisplayName("請假結束日")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CLeaveEndTime { get; set; }

        [DisplayName("請假原由")]
        [Required(ErrorMessage = "必填欄位")]
        public string CReason { get; set; }

        [DisplayName("狀態")]
        public string CCheckStatus { get; set; }

        public int CCheckStatusId { get; set; }

        public virtual TUserDepartment CDepartment { get { return iv_Leave.CDepartment; } set { iv_Leave.CDepartment = value; } }
        public virtual TCheckStatus CCheckStatusNavigation { get { return iv_Leave.CCheckStatusNavigation; } set { iv_Leave.CCheckStatusNavigation = value; } }
        public virtual TUser CEmployee { get { return iv_Leave.CEmployee; } set { iv_Leave.CEmployee = value; } }
        public virtual TLeave CLeaveCategoryNavigation { get { return iv_Leave.CLeaveCategoryNavigation; } set { iv_Leave.CLeaveCategoryNavigation = value; } }
    }


}

