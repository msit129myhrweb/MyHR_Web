
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
        dbMyCompanyContext db = new dbMyCompanyContext();
        private TRepair iv_repair = null;
        private TUser iv_User = null;

        public TRepair repair { get { return iv_repair; } }
        public TUser user { get { return iv_User; } }
        public CReairViewModel(TRepair p,TUser u)
        {
            iv_repair = p;
            iv_User = u;
        }
        public CReairViewModel()
        {
            iv_repair = new TRepair();
            iv_User = new TUser();
        }


        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("維修單號")]
        public int CRepairNumber
        {
            get { return iv_repair.CRepairNumber; }
            set { iv_repair.CRepairNumber = value; }
        }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("員工編號")]
        public int? CEmployeeId
        {

            get { return iv_repair.CEmployeeId; }
            set { iv_repair.CEmployeeId = (int)value; }
        }

        [DisplayName("員工名稱")] 
        public string CEmployeeName
        {
            get
            {
                iv_User.CEmployeeName = db.TUsers.Where(n => n.CEmployeeId == CEmployeeId).Select(n => n.CEmployeeName).FirstOrDefault();
                return iv_User.CEmployeeName;
            }
            set { iv_User.CEmployeeName = value; }
        }



        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("報修申請日期")]
        public DateTime? CAppleDate
        {
            get { return iv_repair.CAppleDate; }
            set { iv_repair.CAppleDate = (DateTime)value; }
        }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("報修類別")]
        public string CRepairCategory
        {
            get { return iv_repair.CRepairCategory; }
            set { iv_repair.CRepairCategory = value; }
        }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("報修地點")]
        public string CLocation
        {
            get { return iv_repair.CLocation; }
            set { iv_repair.CLocation = value; }
        }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("報修內容")]
        public string CContentofRepair
        {
            get { return iv_repair.CContentofRepair; }
            set { iv_repair.CContentofRepair = value; }
        }
        [Required(ErrorMessage = "必填欄位")]
        [DisplayName("連絡電話")]
        public string CPhone
        {
            get { return iv_repair.CPhone; }
            set { iv_repair.CPhone = value; }
        }
        [Required(ErrorMessage = "必填欄位")]
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


        public string employeeName { get { return iv_User.CEmployeeName; } set { iv_User.CEmployeeName = value; } }




    }
}
