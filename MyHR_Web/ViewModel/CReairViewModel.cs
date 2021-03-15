﻿
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CReairViewModel

    {
        private TRepair iv_repair = null;
        public TRepair repair { get { return iv_repair; } }
        public CReairViewModel(TRepair p)
        {
            iv_repair = p;
        }
        public CReairViewModel()
        {
            iv_repair = new TRepair();
        }



        [DisplayName("維修單號")]
        public int CRepairNumber
        {
            get { return iv_repair.CRepairNumber; }
            set { iv_repair.CRepairNumber = value; }
        }

        [DisplayName("員工編號")]
        public int? CEmployeeId
        {

            get { return iv_repair.CEmployeeId; }
            set { iv_repair.CEmployeeId = (int)value; }
        }

        [DisplayName("報修申請日期")]
        public DateTime? CAppleDate
        {
            get { return iv_repair.CAppleDate; }
            set { iv_repair.CAppleDate = (DateTime)value; }
        }

        [DisplayName("報修類別")]
        public string CRepairCategory
        {
            get { return iv_repair.CRepairCategory; }
            set { iv_repair.CRepairCategory = value; }
        }

        [DisplayName("報修地點")]
        public string CLocation
        {
            get { return iv_repair.CLocation; }
            set { iv_repair.CLocation = value; }
        }

        [DisplayName("報修內容")]
        public string CContentofRepair
        {
            get { return iv_repair.CContentofRepair; }
            set { iv_repair.CContentofRepair = value; }
        }

        [DisplayName("連絡電話")]
        public string CPhone
        {
            get { return iv_repair.CPhone; }
            set { iv_repair.CPhone = value; }
        }

        [DisplayName("維修狀態")]
        public byte? CRepairStatus
        {
            get { return iv_repair.CRepairStatus; }
            set { iv_repair.CRepairStatus = (byte)value; }
        }

        //todo Reina
        //[DisplayName("員工")]
        //public virtual TUser Employee {
        //    get { return iv_repair.Employee; }
        //    set { iv_repair.Employee = value; }
        //}







    }
}
