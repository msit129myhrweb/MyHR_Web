﻿using MyHR_Web.Models;
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
        private TTravelExpenseApplication iv_travel = null;

        public TTravelExpenseApplication travel { get { return iv_travel; } }

        public CTravelViewModel(TTravelExpenseApplication t)
        {
            iv_travel = t;
        }
        public CTravelViewModel()
        {
            iv_travel = new TTravelExpenseApplication();
        }
        [DisplayName("差旅費編號")]
        public int CApplyNumber
        {
            get { return iv_travel.CApplyNumber; }
            set { iv_travel.CApplyNumber = value; }
        }
        [Required(ErrorMessage = "部門是必填欄位")]
        [DisplayName("部門")]
        public string CDepartmentName { get; set; }
        public int CDepartmentId
        {
            get 
            {
                iv_travel.CDepartmentId = (int)Enum.Parse(typeof(eDepartment), CDepartmentName);
                return iv_travel.CDepartmentId; 
            }
            set { iv_travel.CDepartmentId = value; }
        }
        [Required(ErrorMessage = "員編是必填欄位")]
        [DisplayName("員編")]
        public int CEmployeeId
        {
            get { return iv_travel.CEmployeeId; }
            set { iv_travel.CEmployeeId = value; }
        }
        [Required(ErrorMessage = "原因是必填欄位")]
        [DisplayName("原因")]
        public string CReason
        {
            get { return iv_travel.CReason; }
            set { iv_travel.CReason = value; }
        }
        [Required(ErrorMessage = "申請日期是必填欄位")]
        [DisplayName("申請日期")]
        public DateTime CApplyDate
        {
            get { return iv_travel.CApplyDate; }
            set { iv_travel.CApplyDate = value; }
        }
        [Required(ErrorMessage = "出差開始時間是必填欄位")]
        [DisplayName("出差開始時間")]
        public DateTime CTravelStartTime
        {
            get { return iv_travel.CTravelStartTime; }
            set { iv_travel.CTravelStartTime = value; }
        }
        [Required(ErrorMessage = "出差結束時間是必填欄位")]
        [DisplayName("出差結束時間")]
        public DateTime CTravelEndTime
        {
            get { return iv_travel.CTravelEndTime; }
            set { iv_travel.CTravelEndTime = value; }
        }
        [Required(ErrorMessage = "差旅費是必填欄位")]
        [DisplayName("差旅費")]
        public decimal CAmont
        {
            get { return iv_travel.CAmont; }
            set { iv_travel.CAmont = value; }
        }

        [DisplayName("審核狀態")]
        public int CCheckStatus
        {
            get { return iv_travel.CCheckStatus; }
            set { iv_travel.CCheckStatus = value; }
        }


    }
}
