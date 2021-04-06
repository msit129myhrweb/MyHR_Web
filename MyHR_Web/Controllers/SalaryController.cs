using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHR_Web.Controllers;
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

            int UserId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));


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

                    var user = MyHR.TUsers.AsEnumerable().FirstOrDefault(x => x.CPassWord.ToString() == Password1.ToString() && x.CEmployeeId == CurrentUserID);

                    if (user != null)
                    {

                        if (UserId == 7)
                        {
                            return RedirectToAction("SalaryList_supervisor");
                        }

                        return RedirectToAction("SalaryList");
                    }
                    else
                    {
                        return RedirectToAction("ConfirmPassword");
                    }
                }
            }
        }

        public IActionResult SalaryList() //薪資單 (一般員工檢視) 檢視上個月
        {

            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
           
            
            var table = MyHR.TUsers
             .Include(c => c.CDepartment)
             .Include(c => c.CJobTitle)
             .Include(c => c.TLeaveApplications)
             .Include(c => c.TTravelExpenseApplications)
             .Include(c => c.TAbsences)
             .Where(c => c.CEmployeeId == UserID).AsEnumerable()
             .Select(c => new CSalaryViewModel
             {
                 CEmployeeName = c.CEmployeeName,
                 CDepartment = c.CDepartment.CDepartment,
                 CEmployeeId = c.CEmployeeId,
                 CJobTitle = c.CJobTitle.CJobTitle,
                 CJobTitleSalary = c.CJobTitle.CJobTitleSalary,
                 CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == (DateTime.Now.Date.Month) - 1 && c.CCheckStatus == 2).Sum(n => (n.CAmont))
                 //CLeaveHours = c.TLeaveApplications.Sum(c=>c.CLeaveHours),
                 //CLeaveHours = c.TLeaveApplications.Where(c=>DateTime.Parse(c.CLeaveStartTime).Month == DateTime.Now.Date.Month).Sum(c => c.CLeaveHours),

             });

            //List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            //foreach (var item in table)
            //{
            //    CSalaryViewModel obj = new CSalaryViewModel()
            //    {
            //        CEmployeeName = item.CEmployeeName,
            //        CDepartment = item.CDepartment,
            //        CEmployeeId = item.CEmployeeId,
            //        CJobTitle = item.CJobTitle,
            //        CJobTitleSalary = item.CJobTitleSalary,
            //        CLeaveHours = item.CLeaveHours
            //    };
            //    T.Add(obj);
            //}

            //------------------------------------------------------------------以下為了取得請假項目的碼


            var table2 = (from i in MyHR.TLeaveApplications.AsEnumerable()
                          where i.CEmployeeId == UserID && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) - 1 //搜尋 "請假起始日"的月為上一個月 (下面也要減)
                          orderby i.CLeaveCategory
                          group i by i.CLeaveCategory into g
                          select new
                          {
                              Category = g.Key,
                              CategoryCount = g.Sum(n => n.CLeaveHours),
                          }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table2)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CSalary_LeaveCate = item.Category,
                    CSalary_LeaveCateCount = (int)item.CategoryCount,
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount, null) //各個假別要付的錢

                };
                T.Add(obj);
                ViewBag.Leave = T;     //這邊算完是放入ViewBag傳送過去的...糟糕
            }




            //------------------------------------------------------------------以下為了取得遲到項目的碼 (30分鐘內44塊錢  一小時內 97)

            var table3 = from i in MyHR.TAbsences   //計算遲到總數
                         where (i.CEmployeeId == UserID && i.CStatus.Contains("遲到") && i.CDate.Value.Month == (DateTime.Now.Date.Month) - 1)
                         group i by i.CStatus into g
                         select new
                         {
                             countLate = g.Count(),
                             below30 = g.Count(c => c.COn.Value.Minutes < 30),
                             up30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),

                             moneybelow30 = g.Count(c => c.COn.Value.Minutes < 30) * 44,
                             moneyup30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 97

                         };

            List<CSalaryViewModel> T2 = new List<CSalaryViewModel>();

            foreach (var item in table3)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    //CountTotalLate = item.countLate,    (取消無相關的欄位)
                    //Count_Latebelow30 = item.below30,
                    //Count_Lateup30 = item.up30,
                    //CAmont_Latebelow30 = item.moneybelow30,
                    //CAmont_Lateup30 = item.moneyup30,
                    CAmont_TAbsense = item.moneybelow30 + item.moneyup30,
                };
                T2.Add(obj);
                ViewBag.Late = T2;
            }

            return View(table.ToList());
        }


        public IActionResult Detail(int? Id) //主管導入詳細資料的薪資單 檢視當月
        {


            var table = MyHR.TUsers
             .Include(c => c.CDepartment)
             .Include(c => c.CJobTitle)
             .Include(c => c.TLeaveApplications)
             .Include(c => c.TTravelExpenseApplications)
             .Include(c => c.TAbsences)
             .Where(c => c.CEmployeeId == Id).AsEnumerable()
             .Select(c => new CSalaryViewModel
             {
                 CEmployeeName = c.CEmployeeName,
                 CDepartment = c.CDepartment.CDepartment,
                 CEmployeeId = c.CEmployeeId,
                 CJobTitle = c.CJobTitle.CJobTitle,
                 CJobTitleSalary = c.CJobTitle.CJobTitleSalary,
                 CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == (DateTime.Now.Date.Month) && c.CCheckStatus == 2).Sum(n => (n.CAmont))
                 //CLeaveHours = c.TLeaveApplications.Sum(c=>c.CLeaveHours),
                 //CLeaveHours = c.TLeaveApplications.Where(c=>DateTime.Parse(c.CLeaveStartTime).Month == DateTime.Now.Date.Month).Sum(c => c.CLeaveHours),

             });

            //------------------------------------------------------------------以下為了取得請假項目的碼


            var table2 = (from i in MyHR.TLeaveApplications.AsEnumerable()
                          where i.CEmployeeId == Id && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) //搜尋 "請假起始日"的月為上一個月 (下面也要減)
                          orderby i.CLeaveCategory
                          group i by i.CLeaveCategory into g
                          select new
                          {
                              Category = g.Key,
                              CategoryCount = g.Sum(n => n.CLeaveHours),
                          }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table2)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CSalary_LeaveCate = item.Category,
                    CSalary_LeaveCateCount = (int)item.CategoryCount,
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount, Id) //各個假別要付的錢

                };
                T.Add(obj);
                ViewBag.Leave = T;     //這邊算完是放入ViewBag傳送過去的...糟糕
            }
            //------------------------------------------------------------------以下為了取得遲到項目的碼

            var table3 = from i in MyHR.TAbsences   //計算遲到總數
                         where (i.CEmployeeId == Id && i.CStatus.Contains("遲到") && i.CDate.Value.Month == (DateTime.Now.Date.Month))   //為啥 =="'遲到'"不行
                         group i by i.CStatus into g
                         select new
                         {
                             countLate = g.Count(),
                             below30 = g.Count(c => c.COn.Value.Minutes < 30),
                             up30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),

                             moneybelow30 = g.Count(c => c.COn.Value.Minutes < 30) * 44,
                             moneyup30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 96

                         };
           

            List<CSalaryViewModel> T2 = new List<CSalaryViewModel>();

            foreach (var item in table3)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                  
                    CAmont_TAbsense = item.moneybelow30 + item.moneyup30,
                };
                T2.Add(obj);
                ViewBag.Late = T2;
            }

            return View(table.ToList());
        }

        public IActionResult SalaryList_supervisor() //所有薪資單(主管)  檢視當月
        {

            var table = MyHR.TUsers
                   .Include(c => c.CDepartment)
                   .Include(c => c.CJobTitle)
                   .Include(c => c.TTravelExpenseApplications)
                   .Include(c => c.TAbsences)
                   .Include(c=>c.TLeaveApplications)
                   .OrderByDescending(c => c.CDepartmentId)
                   .ThenBy(c => c.CJobTitleId)
                   .Where(C => C.COnBoardStatusId == 1).AsEnumerable()
                   .Select(c => new CSalaryViewModel
                   {

                       CDepartment = c.CDepartment.CDepartment,
                       CDepartmentId = c.CDepartment.CDepartmentId,
                       CEmployeeName = c.CEmployeeName,
                       CEmployeeId = c.CEmployeeId,
                       CJobTitle = c.CJobTitle.CJobTitle,
                       Month_Salary = c.CJobTitle.CJobTitleSalary,
                       CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == (DateTime.Now.Date.Month) && c.CCheckStatus == 2).Sum(c => c.CAmont),
                       CAmont_TAbsense = c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes < 30) * 44 + c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 97 - c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),


                       //CAmont_Leave = Leave_ShouldtopayTRYYYYYY(c.CEmployeeId),

                   });
            var table1SQL = table.AsQueryable<CSalaryViewModel>().ToQueryString();
            //------------------------------------------------------------ 請假

            var table2 = (from i in MyHR.TLeaveApplications.AsEnumerable()
                          where /*i.CEmployeeId == UserID && */DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) //當月
                          orderby i.CLeaveCategory
                          group i by i.CLeaveCategory into g
                          select new
                          {
                              Category = g.Key,
                              CategoryCount = g.Sum(n => n.CLeaveHours),
                          }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table2)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                   
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount, null) //各個假別要付的錢

                };
                T.Add(obj);
                ViewBag.Leave = T;     //這邊算完是放入ViewBag傳送過去的...糟糕
            }

            return View(table.ToList());
        }

        //public int Leave_ShouldtopayTRYYYYYY(int id)
        //{
        //    int Leave_Sum = 0;


        //    var table_Leave = (from i in MyHR.TLeaveApplications.AsEnumerable()
        //                  where i.CEmployeeId == id && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) //當月
        //                  orderby i.CLeaveCategory
        //                  group i by i.CLeaveCategory into g
        //                  select new
        //                  {
        //                      Category = g.Key,
        //                      CategoryCount = g.Sum(n => n.CLeaveHours),
        //                  }).ToList();

        //    List<CSalaryViewModel> T = new List<CSalaryViewModel>();

        //    foreach (var item in table_Leave)
        //    {
               
     
        //            int Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount, id); //各個假別要付的錢

        //             Leave_Sum += Leave_HaveToPay;
        //    }

           
        //    return Leave_Sum;
        //}


    


        public int Leave_Shouldtopay(int LeaveCate, int LeaveHours, int? Id)  //計算各個假別必須扣除的總數
        {
            int UserID = 0;

            if (Id != null)
            {
                UserID = (int)Id;
            }
            else
            {
                UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
            }
            double Leave_Sum = 0;
            int MonthWage = (from i in MyHR.TUsers               //取出每人的本薪
                             join j in MyHR.TUserJobTitles
                             on i.CJobTitleId equals j.CJobTitleId
                             where i.CEmployeeId == UserID
                             select j.CJobTitleSalary).FirstOrDefault();
            int HoursWage = (MonthWage / 30 / 8);


            if (LeaveCate == 1) /*病假*/
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

        public IActionResult Count_Leave(int? id)
        {

            int UserID = 0;
            int Month = 0;


            if (id != null)
            {
                UserID = (int)id;
                Month = DateTime.Now.Date.Month;
            }
            else
            {
                UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
                Month = DateTime.Now.Date.Month - 1;
            }

            ViewBag.MONTH = Month;

            var table = (from i in MyHR.TLeaveApplications.AsEnumerable()
                         where i.CEmployeeId == UserID && DateTime.Parse(i.CLeaveStartTime).Month == Month  //搜尋 "請假起始日"的月為上一個月 (上面也要減)
                         orderby i.CLeaveCategory
                         group i by i.CLeaveCategory into g
                         select new
                         {
                             Category = g.Key,
                             CategoryCount = g.Sum(n => n.CLeaveHours),
                         }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CSalary_LeaveCate = item.Category,
                    CSalary_LeaveCateCount = (int)item.CategoryCount,
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount, null) //各個假別要付的錢
                };
                T.Add(obj);
            }

            return PartialView("Count_Leave", T);
        } //

        public IActionResult Count_Absense(int? id) //遲到
        {

            int UserID = 0;
            int Month = 0;

            if (id != null)
            {
                UserID = (int)id;
                Month = DateTime.Now.Date.Month;
            }
            else
            {
                UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
                Month = DateTime.Now.Date.Month - 1;
            }

            ViewBag.MONTH = Month;
            var table = from i in MyHR.TAbsences   //計算遲到總數
                        where (i.CEmployeeId == UserID && i.CStatus.Contains("遲到") && i.CDate.Value.Month == Month)
                        group i by i.CStatus into g
                        select new
                        {
                            countLate = g.Count(),
                            below30 = g.Count(c => c.COn.Value.Minutes < 30),
                            up30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),

                            moneybelow30 = g.Count(c => c.COn.Value.Minutes < 30) * 44,
                            moneyup30 = g.Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 96

                        };

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CountTotalLate = item.countLate,
                    Count_Latebelow30 = item.below30,
                    Count_Lateup30 = item.up30,
                    CAmont_Latebelow30 = item.moneybelow30,
                    CAmont_Lateup30 = item.moneyup30,
                    CAmont_TAbsense = item.moneybelow30 + item.moneyup30,
                };

                T.Add(obj);
            }

            return PartialView("Count_Absense", T);
        }






    }
}
