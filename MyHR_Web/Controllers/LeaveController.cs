﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            //string keyword_StartDay = Request.Form["Leave_Startday"];
            //string keyword_EndDay = Request.Form["Leave_Endday"];

            //if(string.IsNullOrEmpty(keyword_StartDay) && string.IsNullOrEmpty(keyword_EndDay))
            // {

            // } 
            // else
            // {

            // }

            //var table = from i in MyHr.TLeaveApplications
            //            orderby i.CApplyDate descending /*依照申請日期降冪排序*/
            //            select i;
            //List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            //foreach (TLeaveApplication T in table)
            //    list.Add(new TLeaveApplicationViewModel(T));
            //return View(list);


            int useraccount = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));



            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            list = //from i in 
                       MyHr.TLeaveApplications
                        .Include(c => c.CDepartment)
                        .Include(c => c.CLeaveCategoryNavigation)
                        .Include(c => c.CCheckStatusNavigation)
                        .Select(c => new TLeaveApplicationViewModel
                        {
                            CApplyNumber = c.CApplyNumber,
                            CDepartmentId = c.CDepartment.CDepartmentId,
                            CDepartmentName = c.CDepartment.CDepartment,
                            CEmployeeId = c.CEmployeeId,
                            CApplyDate = c.CApplyDate,
                            CLeaveCategoryId = c.CLeaveCategoryNavigation.CLeaveId,
                            CLeaveCategory = c.CLeaveCategoryNavigation.CLeaveCategory,
                            CLeaveStartTime = c.CLeaveStartTime,
                            CLeaveEndTime = c.CLeaveEndTime,
                            CReason = c.CReason,
                            CCheckStatusId = c.CCheckStatusNavigation.CCheckStatusId,
                            CCheckStatus = c.CCheckStatusNavigation.CCheckStatus
                        })
                        .Where(c => c.CEmployeeId == useraccount)
                        .OrderByDescending(c => c.CApplyDate).ToList();
            //orderby i.CApplyDate descending /*依照申請日期降冪排序*/
            //select i;

            //foreach (TLeaveApplication T in table)
            //    list.Add(new TLeaveApplicationViewModel(T));
            return View(list);
            int UserID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

        }

        public IActionResult LeaveCreate()
        {
            return View();
        }

        [HttpPost] /*我有修改TLeaveApplicationViewModel裡面的全域變數名稱*/
        public IActionResult LeaveCreate(TLeaveApplicationViewModel T)
        {
            MyHr.TLeaveApplications.Add(T.Leave);
            MyHr.SaveChanges();
            return RedirectToAction("LeaveList");
        }

        public IActionResult LeaveEdit()
        {
            return View();
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
                    _revised.CLeaveCategory = T.CLeaveCategoryNavigation.CLeaveId;
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
