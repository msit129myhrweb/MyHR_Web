using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CSalaryViewModel
    {
          //TUser
        public int CEmployeeId { get; set; }
        public string CEmployeeName { get; set; }
        public string CEmployeeEnglishName { get; set; }
        public string CPassWord { get; set; }


        //TDepartment

        public int CDepartmentId { get; set; }
        public string CDepartment { get; set; }


        //TJobtitle

        public string CJobTitle { get; set; }
        public int CJobTitleSalary { get; set; }

        //TLeaveApplication




    }
}
