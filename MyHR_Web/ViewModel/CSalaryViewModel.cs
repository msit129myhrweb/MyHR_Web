using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CSalaryViewModel
    {
        dbMyCompanyContext MyHR = new dbMyCompanyContext();
        
          //TUser
        public int CEmployeeId { get; set; }

        [DisplayName("員工姓名")]
        public string CEmployeeName { get; set; }
        public string CEmployeeEnglishName { get; set; }
        public string CPassWord { get; set; }


        //TDepartment

        public int CDepartmentId { get; set; }
        [DisplayName("部門")]
        public string CDepartment { get; set; }


        //TJobtitle
        [DisplayName("職位")]
        public string CJobTitle { get; set; }
        public int CJobTitleSalary { get; set; }

        //TLeaveApplication

        public int? CLeaveHours { get; set; } 


        public int CAmont_Leave { get; set; }

        //TTravel

        public int CAmont_Travel { get; set; }


        



        //TAbsense
        [DisplayName("本月遲到總扣款")]
        public int CAmont_TAbsense { get; set; }

        [DisplayName("本月請假總時數")]
        public int CountTotalLate { get; set; }

        [DisplayName("遲到30分鐘")]
        public int Count_Latebelow30 { get; set; }

        [DisplayName("遲到59分鐘")]
        public int Count_Lateup30 { get; set; }

        public int CAmont_Latebelow30 { get; set; }
       
        public int CAmont_Lateup30 { get; set; }


        //Leave




        //自己創造 Salary

        [DisplayName("請假類別")]
        public int CSalary_LeaveCate { get; set; }
        [DisplayName("請假類別")]
        public string CSalary_LeaveCateName  //下面多設一個變數去承接算出來的種類名稱值。在GET出來。 
        {
            get
            {
                string _CSalary_LeaveCateName = MyHR.TLeaves.Where(n => n.CLeaveId == CSalary_LeaveCate).Select(n => n.CLeaveCategory).FirstOrDefault();
                return _CSalary_LeaveCateName;
            }
            set { CSalary_LeaveCateName = value; }
        }

        [DisplayName("當月請假總時數")]
        public int CSalary_LeaveCateCount { get; set; }

        //[DisplayName("%")]
        //public double CSalary_LeavePercent
        //{
        //    get 
        //    {
        //        double _CSalary_LeavePercent;

        //        switch (CSalary_LeaveCate)
        //        {
        //            case 1:
        //                _CSalary_LeavePercent = 0.5;
        //                break;
        //            case 2:
        //                _CSalary_LeavePercent = 1;
        //                break;
        //            case 3:
        //                _CSalary_LeavePercent = 0.5;
        //                break;
        //            case 4:
        //                _CSalary_LeavePercent = 0;
        //                break;
        //            case 5:
        //                _CSalary_LeavePercent = 0;
        //                break;

        //            default:
        //                _CSalary_LeavePercent = 0;
        //                break;
        //        }
        //         return _CSalary_LeavePercent;
        //    }
        //    set { CSalary_LeavePercent = value; }
        //}

        [DisplayName("扣款明細")]
        public int Leave_HaveToPay { get; set; }


        [DisplayName("當月應付薪水")]
        public int Month_Salary { get; set; }
       
        
       


    }
}
