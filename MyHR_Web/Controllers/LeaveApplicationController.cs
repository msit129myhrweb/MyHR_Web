using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class LeaveApplicationController : Controller
    {
        public IActionResult List()
        {        
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            //IEnumerable<TLeaveApplication> table = null;
            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            dbMyCompanyContext DB = new dbMyCompanyContext();

            // keyword searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string Keyword = Request.Form["txtKeyword"];

                //事由跟申請單號為空
                if (string.IsNullOrEmpty(Keyword))
                {
                    var table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                            join user in (new dbMyCompanyContext()).TUsers on leave.CEmployeeId equals user.CEmployeeId
                            where leave.CDepartmentId == DepId
                            select leave;
                }
                //事由、申請單號都(或其一)有值
                else
                {
                    var table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                            join user in (new dbMyCompanyContext()).TUsers 
                            on leave.CEmployeeId equals user.CEmployeeId
                            where leave.CDepartmentId == DepId &&
                                  leave.CReason.Contains(Keyword) ||
                                  leave.CApplyNumber.ToString().Contains(Keyword)
                            select leave;
                }
            }
            //事由跟申請單號為空白
            else
            {
                var table =from leave in DB.TLeaveApplications
                        join user in DB.TUsers
                        on leave.CEmployeeId equals user.CEmployeeId
                        where leave.CDepartmentId == DepId
                        select new {
                            user.CEmployeeName,
                            leave.CApplyDate,
                            leave.CApplyNumber,
                            leave.CCheckStatus,
                            leave.CLeaveCategory,
                            leave.CLeaveStartTime,
                            leave.CLeaveEndTime,
                            leave.CEmployeeId,
                            leave.CReason
                        };

                foreach (var item in table)
                {
                    TLeaveApplicationViewModel newObj = new TLeaveApplicationViewModel()
                    {
                        CApplyNumber = item.CApplyNumber,
                        CApplyDate = item.CApplyDate,
                        employeeName = item.CEmployeeName,
                        CEmployeeId=item.CEmployeeId,
                        CReason=item.CReason,
                        CLeaveStartTime=item.CLeaveStartTime,
                        CLeaveEndTime=item.CLeaveEndTime,
                        CLeaveCategory=item.CLeaveCategory,
                        CCheckStatus=item.CCheckStatus

                    };
                    list.Add(newObj);
                }
            }
           return View(list) ;
        }
        #region Edit
        //勾選通過
        public JsonResult getPassString(string d)
        {
            string[] ids = d.Split('\\', '"', '[', ',', ']');
            List<int> list = new List<int>();
            foreach (var item in ids)
            {
                if (item != "")
                {
                    list.Add(int.Parse(item));
                }
            }
            foreach (var i in list)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(c => c.CApplyNumber == i);
                if (leave.CCheckStatus == 1)
                {
                    if (leave != null)
                    {
                        leave.CCheckStatus = 2;
                        db.SaveChanges();
                    }

                }
            }
            return Json(d);
        }
        //勾選退件
        public JsonResult getFailString(string d)
        {
            string[] ids = d.Split('\\', '"', '[', ',', ']');
            List<int> list = new List<int>();
            foreach (var item in ids)
            {
                if (item != "")
                {
                    list.Add(int.Parse(item));
                }
            }
            foreach (var i in list)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(c=>c.CApplyNumber==i);
                if (leave.CCheckStatus == 1)
                {
                    if (leave != null)
                    {
                        leave.CCheckStatus = 3;
                        db.SaveChanges();
                    }

                }
            }
            return Json(d);
        }



        //通過或退件
        public IActionResult pass(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(l => l.CApplyNumber == id);
                if (leave != null)
                {
                    if (leave.CCheckStatus==1)
                    {
                        leave.CCheckStatus = 2;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult fail(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(l => l.CApplyNumber == id);
                if (leave != null)
                {
                    if (leave.CCheckStatus == 1)
                    {
                        leave.CCheckStatus = 3;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("List");
        }
    }
    #endregion
}
