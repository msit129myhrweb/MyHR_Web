using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany_.NetCore_Janna.Controllers
{
    public class SalaryController : Controller
    {
        dbMyCompanyContext MyHR = new dbMyCompanyContext();

     

        public IActionResult ConfirmPassword()
        {
            ViewBag.Name = HttpContext.Session.GetString("CURRENT_LOGINED_USERENNAME");
            return PartialView();
        }
        [HttpPost]
        public IActionResult ConfirmPassword(CSalaryLoginViewModel p)
        {
            string Password1 = Request.Form["Password"].ToString();
            if (string.IsNullOrEmpty(Password1))
            {
                return RedirectToAction("ConfirmPassword");
            }
            else
            {
                using (dbMyCompanyContext MyHR = new dbMyCompanyContext())
                {
                    int CurrentUserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));

                    var user = MyHR.TUsers.AsEnumerable().FirstOrDefault(x => int.Parse(x.CPassWord) == int.Parse(Password1) && x.CEmployeeId == CurrentUserID);

                    if (user != null)
                    {
                        return RedirectToAction("SalaryList");
                    }
                    else
                    {
                        return RedirectToAction("ConfirmPassword");
                    }
                }
            }
        }


        public IActionResult SalaryList()
        {

            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
            var table = MyHR.TUsers
             .Include(c => c.CDepartment)
             .Include(c => c.CJobTitle)
             .Include(d=>d.TLeaveApplications)
             .Where(c => c.CEmployeeId == UserID).AsEnumerable()
             .Select(c => new CSalaryViewModel
             {
                 CEmployeeName = c.CEmployeeName,
                 CDepartment = c.CDepartment.CDepartment,
                 CEmployeeId = c.CEmployeeId,
                 CJobTitle = c.CJobTitle.CJobTitle,
                 CJobTitleSalary = c.CJobTitle.CJobTitleSalary,
                 //CLeaveHours = c.TLeaveApplications.Sum(c=>c.CLeaveHours),
                 CLeaveHours = c.TLeaveApplications.Where(c=>DateTime.Parse(c.CLeaveStartTime).Month == DateTime.Now.Date.Month).Sum(c => c.CLeaveHours),
                
             });

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();
            
            foreach (var item in table)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CEmployeeName = item.CEmployeeName,
                    CDepartment = item.CDepartment,
                    CEmployeeId = item.CEmployeeId,
                    CJobTitle = item.CJobTitle,
                    CJobTitleSalary = item.CJobTitleSalary,
                    CLeaveHours=item.CLeaveHours
                    
                };
                T.Add(obj);
            }

          //------------------------------------------------------------------
            return View(table.ToList());
        }


        public IActionResult Count_Leave()
        {
            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));

            var table = (from i in MyHR.TLeaveApplications.AsEnumerable()
                        where i.CEmployeeId == UserID && DateTime.Parse(i.CLeaveStartTime).Month == DateTime.Now.Date.Month  //搜尋 "請假起始日"的月為當月
                        orderby i.CLeaveCategory
                        group i by i.CLeaveCategory into g
                        
                        
                        select new
                        {
                            Category = g.Key,
                            CategoryCount = g.Sum(n => n.CLeaveHours),
                            }).ToList();



            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach(var item in table)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CSalary_LeaveCate = item.Category,
                    CSalary_LeaveCateCount = (int)item.CategoryCount,
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category,(int)item.CategoryCount) //各個假別要付的錢
                };
                T.Add(obj);
            }

            return PartialView("Count_Leave",T);
        }

      public int Leave_Shouldtopay(int LeaveCate, int LeaveHours)  //計算各個假別必須扣除的總數
        {
           
            double Leave_Sum = 0;

            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
            int MonthWage = (from i in MyHR.TUsers               //取出每人的本薪
                             join j in MyHR.TUserJobTitles
                             on i.CJobTitleId equals j.CJobTitleId
                             where i.CEmployeeId == UserID
                             select j.CJobTitleSalary).FirstOrDefault();
            int HoursWage = (MonthWage / 30 / 8);


            if(LeaveCate == 1) /*病假*/
            {
                Leave_Sum = LeaveHours * 0.5 * HoursWage;
            }
            else if (LeaveCate == 2)/*事假*/
            {
                Leave_Sum = LeaveHours * 1 * HoursWage;
            }
            else if (LeaveCate == 3)/*生理假*/
            {
                Leave_Sum = LeaveHours * 0.5 * HoursWage;
            }
            else if (LeaveCate == 4) /*喪假*/
            {
                Leave_Sum = LeaveHours * 0 * HoursWage;
            }
            else if (LeaveCate == 5)/*特休*/
            {
                Leave_Sum = LeaveHours * 0 * HoursWage;
            }     
            return (int)Leave_Sum;


        }

    


    }
}
