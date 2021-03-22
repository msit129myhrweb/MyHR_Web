﻿using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class TLeaveApplicationViewModel
    {
        private TLeaveApplication iv_Leave = null;
        private TUser iv_User = null;

        public TLeaveApplication product { get { return iv_Leave; } }
        public TUser user { get { return iv_User; } }

        public TLeaveApplicationViewModel(TLeaveApplication p, TUser u)
        {
            iv_Leave = p;
            iv_User = u;
        }

        //其實不需要---------------------------------------------------
        public TLeaveApplicationViewModel(TLeaveApplication p)
        {
            iv_Leave = p;
        }
        //------------------------------------------------------------

        public TLeaveApplicationViewModel()
        {
            iv_Leave = new TLeaveApplication();
            iv_User = new TUser();
        }


        [DisplayName("申請單號")]
        public int CApplyNumber { get {return iv_Leave.CApplyNumber; } set {iv_Leave.CApplyNumber=value; } }
    
        [DisplayName("部門名稱")]
        public int CDepartmentId { get { return iv_Leave.CDepartmentId; } set { iv_Leave.CDepartmentId = value; } }
        [DisplayName("員工編號")]
        public int CEmployeeId { get { return iv_Leave.CEmployeeId; } set { iv_Leave.CEmployeeId = value; } }
        [DisplayName("申請日")]
        public DateTime CApplyDate { get { return iv_Leave.CApplyDate; } set { iv_Leave.CApplyDate = value; } }
        [DisplayName("申請類別")]
        public int CLeaveCategory { get { return iv_Leave.CLeaveCategory; } set { iv_Leave.CLeaveCategory = value; } }
        [DisplayName("請假起始日")]
        public DateTime CLeaveStartTime { get { return iv_Leave.CLeaveStartTime; } set { iv_Leave.CLeaveStartTime = value; } }
        [DisplayName("請假結束日")]
        public DateTime CLeaveEndTime { get { return iv_Leave.CLeaveEndTime; } set { iv_Leave.CLeaveEndTime = value; } }
        [DisplayName("請假原由")]
        public string CReason { get { return iv_Leave.CReason; } set { iv_Leave.CReason = value; } }
        [DisplayName("狀態")]
        public int CCheckStatus { get { return iv_Leave.CCheckStatus; } set { iv_Leave.CCheckStatus = value; } }

        public virtual TCheckStatus CCheckStatusNavigation { get { return iv_Leave.CCheckStatusNavigation; } set { iv_Leave.CCheckStatusNavigation = value; } }
        public virtual TUser CEmployee { get { return iv_Leave.CEmployee; } set { iv_Leave.CEmployee = value; } }
        public virtual TLeave CLeaveCategoryNavigation { get { return iv_Leave.CLeaveCategoryNavigation; } set { iv_Leave.CLeaveCategoryNavigation = value; } }
        [DisplayName("員工姓名")]

        public string employeeName { get{return iv_User.CEmployeeName ; } set{iv_User.CEmployeeName=value ; }}

    }


}

