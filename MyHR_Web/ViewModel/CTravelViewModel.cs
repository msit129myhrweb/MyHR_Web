using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CTravelViewModel
    { 
        [DisplayName("差旅費編號")]
        public int CApplyNumber { get; set; }

        [DisplayName("部門")]
        public int CDepartmentId { get; set; }
        [Required(ErrorMessage = "員編是必填欄位")]
        [DisplayName("員編")]
        public int CEmployeeId { get; set; }
        [Required(ErrorMessage = "原因是必填欄位")]
        [DisplayName("原因")]
        public string CReason { get; set; }
        [Required(ErrorMessage = "申請日期是必填欄位")]
        [DisplayName("申請日期")]
        public DateTime? CApplyDate { get; set; }
        [Required(ErrorMessage = "出差開始時間是必填欄位")]
        [DisplayName("出差開始時間")]
        public DateTime? CTravelStartTime { get; set; }
        [Required(ErrorMessage = "出差結束時間是必填欄位")]
        [DisplayName("出差結束時間")]
        public DateTime? CTravelEndTime { get; set; }
        [Required(ErrorMessage = "差旅費是必填欄位")]
        [DisplayName("差旅費")]
        public decimal CAmont { get; set; }
        [DisplayName("審核狀態")]
        public int CCheckStatus { get; set; }


    }
}
