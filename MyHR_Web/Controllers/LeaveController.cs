using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class LeaveController : Controller
    {
        dbMyCompanyContext MyHr = new dbMyCompanyContext();

        public IActionResult LeaveList()  //請假清單查詢
        {
           

            //string keyword_EndDay = Request.Form["Leave_Endday"];

            int UserID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            IEnumerable<TLeaveApplication> table = null;

            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string keyword_StartDay = Request.Form["Leave_Startday"];
                string keyword_EndDay = Request.Form["Leave_Endday"];

                string whereCommond;

                if (!string.IsNullOrEmpty(keyword_StartDay) && !string.IsNullOrEmpty(keyword_EndDay))//2個日期不得為空
                {
                    table = from i in MyHr.TLeaveApplications
                            where i.CEmployeeId == UserID && i.CApplyDate >= DateTime.Parse(keyword_StartDay) && i.CApplyDate <= DateTime.Parse(keyword_EndDay)
                            orderby i.CApplyDate descending /*依照申請日期降冪排序*/
                            select i;
                }

                else if (!string.IsNullOrEmpty(keyword_StartDay))//起始日期不得為空
                {
                    table = from i in MyHr.TLeaveApplications
                            where i.CEmployeeId == UserID && i.CApplyDate >= DateTime.Parse(keyword_StartDay)
                            orderby i.CApplyDate descending 
                            select i;

                }
                else if (!string.IsNullOrEmpty(keyword_EndDay))//終止日期不得為空
                {
                    table = from i in MyHr.TLeaveApplications
                            where i.CEmployeeId == UserID && i.CApplyDate <= DateTime.Parse(keyword_EndDay)
                            orderby i.CApplyDate descending 
                            select i;
                }

                else
                {
                    table = from i in MyHr.TLeaveApplications
                            where i.CEmployeeId == UserID
                            orderby i.CApplyDate descending /*依照申請日期降冪排序*/
                            select i;
                }

            }
            else
    {           //全選
                table = from i in MyHr.TLeaveApplications
                where i.CEmployeeId == UserID
                orderby i.CApplyDate descending /*依照申請日期降冪排序*/
                select i;
            }

            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            foreach (TLeaveApplication T in table)
                list.Add(new TLeaveApplicationViewModel(T));
            return View(list);


            //string keyword_Start = Request.Form["Leave_Startday"].ToString();
            //string keyword_End = Request.Form["Leave_Endday"].ToString();

            //    if(!string.IsNullOrEmpty(keyword_Start) && !string.IsNullOrEmpty(keyword_End))
            //{
            //    var list = MyHr.TLeaveApplications.Select(c => c.CApplyDate.ToShortTimeString() <= 'keyword_End' AND c.CApplyDate.ToShortTimeString() > keyword_Start);

            //}


            //******************************************************************************************************
            //int useraccount = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            //List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            //list = //from i in 
            //           MyHr.TLeaveApplications
            //            .Include(c => c.CDepartment)
            //            .Include(c => c.CLeaveCategoryNavigation)
            //            .Include(c => c.CCheckStatusNavigation)
            //            .Select(c => new TLeaveApplicationViewModel
            //            {
            //                CApplyNumber = c.CApplyNumber,
            //                CDepartmentId = c.CDepartment.CDepartmentId,
            //                CDepartmentName = c.CDepartment.CDepartment,
            //                CEmployeeId = c.CEmployeeId,
            //                CApplyDate = c.CApplyDate,
            //                CLeaveCategoryId = c.CLeaveCategoryNavigation.CLeaveId,
            //                CLeaveCategory = c.CLeaveCategoryNavigation.CLeaveCategory,
            //                CLeaveStartTime = c.CLeaveStartTime,
            //                CLeaveEndTime = c.CLeaveEndTime,
            //                CReason = c.CReason,
            //                CCheckStatusId = c.CCheckStatusNavigation.CCheckStatusId,
            //                CCheckStatus = c.CCheckStatusNavigation.CCheckStatus
            //            })
            //            .Where(c => c.CEmployeeId == useraccount)
            //            .OrderByDescending(c => c.CApplyDate).ToList();

            //return View(list);
            //int UserID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            //******************************************************************************************************

        }

        public IActionResult LeaveCreate()
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData["Today"] = DateTime.Now.ToString("yyyy/MM/dd");


            var departmentsQuery = from d in MyHr.TUserDepartments
                                   orderby d.CDepartmentId // Sort by name.
                                   select d;

            //DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
            //            "DepartmentID", "Name", selectedDepartment);





            return View();
        }

        [HttpPost] /*我有修改TLeaveApplicationViewModel裡面的全域變數名稱*/
        public IActionResult LeaveCreate(TLeaveApplicationViewModel T)
        {
            //using (var context = new dbMyCompanyContext())
            //{
            //    var item = new item { };
            //    context.Blogs.Add(blog);
            //    context.SaveChanges();
            //}


            //List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            //list = MyHr.TLeaveApplications


            MyHr.TLeaveApplications.Add(T.Leave);
            MyHr.SaveChanges();
            return RedirectToAction("LeaveList");
        }

      

        public IActionResult Delete(int? Id) //刪除
        {
            if (Id != null)
            {
                TLeaveApplication T = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == Id);
                if (T != null)
                {
                    MyHr.TLeaveApplications.Remove(T);
                    MyHr.SaveChanges();
                }
            }

            return RedirectToAction("LeaveList");
        }


        public IActionResult Edit(int? Id)
        {

            ViewData["Today"] = DateTime.Now.ToString("yyyy/MM/dd");
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);


            if (Id != null)
            {
                TLeaveApplication T = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == Id);
                if (T != null)
                {
                    return View(new TLeaveApplicationViewModel(T));
                }
            }
            return RedirectToAction("LeaveList");
        }

        [HttpPost]
        public IActionResult Edit(TLeaveApplicationViewModel T)
        {
            if (T != null)
            {
                TLeaveApplication _revised = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == T.CApplyNumber);

                if (_revised != null)
                {
                    _revised.CApplyDate = T.CApplyDate;
                    _revised.CLeaveCategory = T.CLeaveCategory;
                    _revised.CLeaveStartTime = T.CLeaveStartTime;
                    _revised.CLeaveEndTime = T.CLeaveEndTime;
                    _revised.CReason = T.CReason;

                    MyHr.SaveChanges();
                }
            }

            return RedirectToAction("LeaveList");
        }




    }
}
