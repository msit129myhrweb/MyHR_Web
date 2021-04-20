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
using System.Net.Mail;
using System.Net;
using MyHR_Web.MyClass;

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

                        else 
                        { 
                            return RedirectToAction("Salary_Search");
                        }

                      
                    }
                    else
                    {
                        return RedirectToAction("ConfirmPassword");
                    }
                }
            }
        }


        public IActionResult Salary_Search()
        {
            MyHR.TUsers.ToList();
            MyHR.TUserDepartments.ToList();
            MyHR.TTravelExpenseApplications.ToList();
            MyHR.TAbsences.ToList();
            MyHR.TUserJobTitles.ToList();
            MyHR.TLeaveApplications.ToList();
            int MONTH = 3;
            int UserID = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User).CEmployeeId;


            var table = MyHR.TUsers.Local
                   .Where(C => C.CEmployeeId == UserID).AsEnumerable()
                   .Select(c => new CSalaryViewModel
                   {
                       CDepartment = c.CDepartment.CDepartment,
                       CDepartmentId = c.CDepartment.CDepartmentId,
                       CEmployeeName = c.CEmployeeName,
                       CEmployeeId = c.CEmployeeId,
                       CJobTitle = c.CJobTitle.CJobTitle,
                       Month_Salary = c.CJobTitle.CJobTitleSalary,
                       CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == MONTH && c.CCheckStatus == 2).Sum(c => c.CAmont),
                       CAmont_TAbsense = c.TAbsences
                       .Where(p => p.CDate.Value.Month == MONTH && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes < 30) * 44 + c.TAbsences
                       .Where(p => p.CDate.Value.Month == MONTH && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 97 - c.TAbsences
                       .Where(p => p.CDate.Value.Month == MONTH && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),
                       CAmont_Leave = Leave_ShouldtopayforSearch(c.CEmployeeId,2021,3)

                   }).ToList();

            foreach (var item in table)
            {
                item.CSalary_Total = item.Month_Salary + item.CAmont_Travel - item.CAmont_TAbsense - item.CAmont_Leave;
                item.CSub_Total = item.CAmont_TAbsense + item.CAmont_Leave;
            }


            return View(table);

        } //員工檢視薪資(VIEW)

       
        public IActionResult PreviousSalary(int? YEAR, int? MONTH)
        {


            if (YEAR != null)
            {
                YEAR = YEAR;
            }
            else
            {
                YEAR = 2021;
            }


            

            MyHR.TUsers.ToList();
            MyHR.TUserDepartments.ToList();
            MyHR.TTravelExpenseApplications.ToList();
            MyHR.TAbsences.ToList();
            MyHR.TUserJobTitles.ToList();
            MyHR.TLeaveApplications.ToList();

            int UserID = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User).CEmployeeId;



            List<List<CSalaryViewModel>> list = new List<List<CSalaryViewModel>>();

            for (int i = 1; i <= MONTH; i++)
            {
                var table = (MyHR.TUsers.Local
                  .Where(C => C.CEmployeeId == UserID).AsEnumerable()
                  .Select(c => new CSalaryViewModel
                  {
                      CDepartment = c.CDepartment.CDepartment,
                      CDepartmentId = c.CDepartment.CDepartmentId,
                      CEmployeeName = c.CEmployeeName,
                      CEmployeeId = c.CEmployeeId,
                      CJobTitle = c.CJobTitle.CJobTitle,
                      Month_Salary = c.CJobTitle.CJobTitleSalary,
                      CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Year == YEAR && c.CTravelStartTime.Value.Month == i && c.CCheckStatus == 2).Sum(c => c.CAmont),
                      CAmont_TAbsense = c.TAbsences
                      .Where(p => p.CDate.Value.Year == YEAR && p.CDate.Value.Month == i && p.CStatus == "遲到")
                      .Count(c => c.COn.Value.Minutes < 30) * 44 + c.TAbsences
                      .Where(p => p.CDate.Value.Year == YEAR && p.CDate.Value.Month == i && p.CStatus == "遲到")
                      .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 97 - c.TAbsences
                      .Where(p => p.CDate.Value.Year == YEAR &&p.CDate.Value.Month == i && p.CStatus == "遲到")
                      .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),
                      CAmont_Leave = Leave_ShouldtopayforSearch(c.CEmployeeId, (int)YEAR, (int)i)

                  })).ToList();

                foreach (var item in table)
                {
                    item.CSalary_Total = item.Month_Salary + item.CAmont_Travel - item.CAmont_TAbsense - item.CAmont_Leave;
                }


                list.Add(table);
            }


            ViewBag.Year = YEAR;
            return PartialView("PreviousSalary", list);

        }   //AJAX 員工檢視過去薪資 (PA)

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
                 CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == (DateTime.Now.Date.Month) - 1 && c.CTravelStartTime.Value.Year == (DateTime.Now.Date.Year) && c.CCheckStatus == 2).Sum(n => (n.CAmont))
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
                          where i.CEmployeeId == UserID && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) - 1 && DateTime.Parse(i.CLeaveStartTime).Year ==DateTime.Now.Date.Year && i.CCheckStatus == 2 //搜尋 "請假起始日"的月為上一個月 (下面也要減)
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
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount) //各個假別要付的錢

                };
                T.Add(obj);
                ViewBag.Leave = T;     //這邊算完是放入ViewBag傳送過去的...糟糕
            }

            var a = T.ToList();


            //------------------------------------------------------------------以下為了取得遲到項目的碼 (30分鐘內44塊錢  一小時內 97)

            var table3 = from i in MyHR.TAbsences   //計算遲到總數
                         where (i.CEmployeeId == UserID && i.CStatus.Contains("遲到") && i.CDate.Value.Month == (DateTime.Now.Date.Month) - 1 && i.CDate.Value.Year == DateTime.Now.Date.Year)
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


        public IActionResult Detail(int Id) //主管導入詳細資料的薪資單 檢視當月
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
                          where i.CEmployeeId == Id && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) && i.CCheckStatus == 2  //搜尋 "請假起始日"的月為上一個月 (下面也要減)
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
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount) //各個假別要付的錢

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
            MyHR.TUsers.ToList();
            MyHR.TUserDepartments.ToList();
            MyHR.TTravelExpenseApplications.ToList();
            MyHR.TAbsences.ToList();
            MyHR.TLeaveApplications.ToList();


            var table = MyHR.TUsers.Local
                   //.Include(c => c.CDepartment)
                   //.Include(c => c.CJobTitle)
                   //.Include(c => c.TTravelExpenseApplications)
                   //.Include(c => c.TAbsences)
                   //.Include(c => c.TLeaveApplications)
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
                       CAmont_Leave = Leave_ShouldtopayTRYYYYYY(c.CEmployeeId)

                   });




            var Dept = MyHR.TUserDepartments.Distinct().ToList();  //傳送部門下拉是選單
            ViewBag.DEPT = Dept;
            var JobTitle = MyHR.TUserJobTitles.Distinct().ToList(); //傳送職位下拉是選單
            ViewBag.JOBTITLE = JobTitle;

            return View(table.ToList());
        }

        public int Leave_ShouldtopayTRYYYYYY(int id)
        {
            int Leave_Sum = 0;


            var table_Leave = (from i in MyHR.TLeaveApplications.AsEnumerable()
                               where i.CEmployeeId == id && DateTime.Parse(i.CLeaveStartTime).Month == (DateTime.Now.Date.Month) && i.CCheckStatus == 2 //當月
                               orderby i.CLeaveCategory
                               group i by i.CLeaveCategory into g
                               select new
                               {
                                   Category = g.Key,
                                   CategoryCount = g.Sum(n => n.CLeaveHours),
                               }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table_Leave)
            {


                int Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount); //各個假別要付的錢

                Leave_Sum += Leave_HaveToPay;
            }


            return Leave_Sum;
        }  //方法: 計算各假別需扣的錢


        public int Leave_ShouldtopayforSearch(int id, int year,int month)
        {
            int Leave_Sum = 0;


            var table_Leave = (from i in MyHR.TLeaveApplications.AsEnumerable()
                               where i.CEmployeeId == id && DateTime.Parse(i.CLeaveStartTime).Year == year && DateTime.Parse(i.CLeaveStartTime).Month == month && i.CCheckStatus == 2 //當月
                               orderby i.CLeaveCategory
                               group i by i.CLeaveCategory into g
                               select new
                               {
                                   Category = g.Key,
                                   CategoryCount = g.Sum(n => n.CLeaveHours),
                               }).ToList();

            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table_Leave)
            {


                int Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount); //各個假別要付的錢

                Leave_Sum += Leave_HaveToPay;
            }


            return Leave_Sum;
        }  //方法: 計算各假別需扣的錢
        public int Leave_Shouldtopay(int LeaveCate, int LeaveHours)  //計算各個假別必須扣除的總數
        {


            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));

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

        public IActionResult Count_Leave(int? id)    //請假
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
                         where i.CEmployeeId == UserID && DateTime.Parse(i.CLeaveStartTime).Month == Month && DateTime.Parse(i.CLeaveStartTime).Year == DateTime.Now.Date.Year && i.CCheckStatus == 2  //搜尋 "請假起始日"的月為上一個月 (上面也要減)
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
                    Leave_HaveToPay = Leave_Shouldtopay(item.Category, (int)item.CategoryCount) //各個假別要付的錢
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
                        where (i.CEmployeeId == UserID && i.CStatus.Contains("遲到") && i.CDate.Value.Month == Month) &&i.CDate.Value.Year == DateTime.Now.Date.Year
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


        public IActionResult Mutiple_search(int? DEPT, int? TITLE)
        {

            MyHR.TUsers.ToList();
            MyHR.TUserDepartments.ToList();
            MyHR.TTravelExpenseApplications.ToList();
            MyHR.TAbsences.ToList();
            MyHR.TLeaveApplications.ToList();
            MyHR.TUserJobTitles.ToList();

            var table = MyHR.TUsers.Local
                   //.Include(c => c.CDepartment)
                   //.Include(c => c.CJobTitle)
                   //.Include(c => c.TTravelExpenseApplications)
                   //.Include(c => c.TAbsences)
                   //.Include(c => c.TLeaveApplications)
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
                       CJobTitleId = c.CJobTitle.CJobTitleId,
                       Month_Salary = c.CJobTitle.CJobTitleSalary,
                       CAmont_Travel = (int)c.TTravelExpenseApplications.Where(c => c.CTravelStartTime.Value.Month == (DateTime.Now.Date.Month) && c.CCheckStatus == 2).Sum(c => c.CAmont),
                       CAmont_TAbsense = c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes < 30) * 44 + c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59) * 97 - c.TAbsences
                       .Where(p => p.CDate.Value.Month == DateTime.Now.Date.Month && p.CStatus == "遲到")
                       .Count(c => c.COn.Value.Minutes > 30 && c.COn.Value.Minutes < 59),
                       CAmont_Leave = Leave_ShouldtopayTRYYYYYY(c.CEmployeeId),
                   }).Where(a =>
                   (DEPT != null ? a.CDepartmentId == DEPT : true) &&
                   (TITLE != null ? a.CJobTitleId == TITLE : true)).ToList();

            
            foreach (var item in table)
            {
                item.CSalary_Total = item.Month_Salary + item.CAmont_Travel - item.CAmont_TAbsense - item.CAmont_Leave;
            }




            return PartialView("Mutiple_search", table);
         
        }

        [HttpPost]
        public IActionResult Mutiple_CHARTsearch([FromBody] SalaryChar_Json x)   //回傳圓餅動態Chart.js
        {
            if (x.money.Count != 0)
            {
                List<SalaryChar_Json> list = new List<SalaryChar_Json>();

                SalaryChar_Json obj = new SalaryChar_Json()
                {

                    money = x.money,
                    name = x.name,


                };
                list.Add(obj);
                return PartialView("Mutiple_CHARTsearch", list);
            }
            else
            {

                return StatusCode(500);

            }



        }


        [HttpPost]
        public IActionResult PreviousSalary_CHARTAnalysis([FromBody] SalaryChart_BarJson x)   //Area 圖
        {
            string[] MonthArr = { "January", "February", "March", "April", "May" ,
                    "June", "July", "August", "September", "October", "November", "December" };


            var a  = MonthArr.Take(int.Parse(x.Month));


            if (x.money.Count != 0)
            {
                List<SalaryChartsss_BarJson> list = new List<SalaryChartsss_BarJson>();

                SalaryChartsss_BarJson obj = new SalaryChartsss_BarJson()
                {
                    
                    money = x.money,
                    leave=x.leave,
                    Monthh =  a.ToList()
                    
                   
                };
                list.Add(obj);
                return PartialView("PreviousSalary_CHARTAnalysis", list);
            }
            else
            {
                return StatusCode(404);
            }
        }

        public IActionResult Mail_Click(string ID)
        {
            string a = ID;
            string[] ids = a.Split('\\', '"', '[', ',', ']');

            List<int> list = new List<int>();
            foreach (var item in ids)
            {
                if (item != "")
                {
                    list.Add(int.Parse(item));
                }
            }

            foreach (var i in list)   //Check box 有勾選的員工ID值
            {
                var Id_Table = MyHR.TUsers.Where(c => c.CEmployeeId == i).Select(c => c.CEmail).FirstOrDefault();

                if (Id_Table != null)
                {
                    try
                    {
                        System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                        string PEOPLE = Id_Table.ToString();

                        msg.To.Add(PEOPLE);
                        //msg.To.Add("b@b.com");可以發送給多人
                        //msg.CC.Add("c@c.com");
                        //msg.CC.Add("c@c.com");可以抄送副本給多人 
                        //這裡可以隨便填，不是很重要
                        msg.From = new MailAddress("msit129hellowork@gmail.com", "HELLOWORK公司", System.Text.Encoding.UTF8);
                        /* 上面3個參數分別是發件人地址（可以隨便寫），發件人姓名，編碼*/
                        msg.Subject = "4月份薪資";//郵件標題
                        msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼
                        msg.Body = "本月薪資已發放，請至個人帳戶進行確認。"; //郵件內容
                        msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                        /*     msg.Attachments.Add(new Attachment(@"D:\test2.docx")); */ //附件
                        msg.IsBodyHtml = true;//是否是HTML郵件 
                                              //msg.Priority = MailPriority.High;//郵件優先級 

                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential("msit129hellowork@gmail.com", "izougqdehrjrufoh"); //這裡要填正確的帳號跟密碼
                        client.Host = "smtp.gmail.com"; //設定smtp Server
                        client.Port = 25; //設定Port
                        client.EnableSsl = true; //gmail預設開啟驗證
                        client.Send(msg); //寄出信件
                        client.Dispose();
                        msg.Dispose();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
               
            }
               return RedirectToAction("SalaryList_supervisor");
        } //傳送MAIL

        public IActionResult PreviousSalary_CHARTforLeave(int Year, int Month)
        {


            if (Year != null)
            {
                Year = Year;
            }
            else
            {
                Year = 2021;
            }

            Month = 7;

            MyHR.TUsers.ToList();
            MyHR.TUserDepartments.ToList();
            MyHR.TTravelExpenseApplications.ToList();
            MyHR.TAbsences.ToList();
            MyHR.TUserJobTitles.ToList();
            MyHR.TLeaveApplications.ToList();

            int UserID = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User).CEmployeeId;



            List<List<LeaveChart_Salary>> list = new List<List<LeaveChart_Salary>>();

            for (int i = 1; i <= Month; i++)
            {
                var table = (MyHR.TUsers.Local
                  .Where(C => C.CEmployeeId == UserID).AsEnumerable()
                  .Select(c => new LeaveChart_Salary
                  {
                      AbsenceCount = c.TAbsences
                      .Count(p => p.CDate.Value.Year == Year && p.CDate.Value.Month == i /*&& p.CStatus == "遲到"*/),

                      LeaveCount = c.TLeaveApplications
                      .Count(p => DateTime.Parse(p.CLeaveStartTime).Year == Year && DateTime.Parse(p.CLeaveStartTime).Month == i /*&& p.CCheckStatus == 2*/)


                  })).ToList();


                list.Add(table);
            }


            //ViewBag.Year = YEAR;




            return PartialView("PreviousSalary_CHARTforLeave");
        }


    }
}
