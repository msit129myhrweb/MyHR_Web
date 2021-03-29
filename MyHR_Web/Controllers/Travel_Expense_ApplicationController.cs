using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using Newtonsoft.Json;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class Travel_Expense_ApplicationController : Controller
    {
        public IActionResult List()
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            //IEnumerable<TTravelExpenseApplication> table = null;
            List<Travel_Expense_ApplicationViewModel> list = new List<Travel_Expense_ApplicationViewModel>();
            dbMyCompanyContext DB = new dbMyCompanyContext();

            //searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string AppNum = Request.Form["txtAppNum"];
                string Id = Request.Form["txtId"];
                string Name = Request.Form["txtName"];
                string Reason = Request.Form["txtReason"];

                //全部條件為空白
                if (string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId
                                select travel;

                    //todo inner join TUser Name

                }
                //其一有值
                else if (!string.IsNullOrEmpty(AppNum) || !string.IsNullOrEmpty(Id) || !string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) ||
                                      travel.CApplyNumber.ToString().Contains(AppNum) ||
                                      travel.CEmployeeId.ToString().Contains(Id) ||
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
                ////其二有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
                ////其三有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
                //全部有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
            }
            //全部條件為空白
            else
            {
                var table = from travel in DB.TTravelExpenseApplications
                            join user in DB.TUsers
                            on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId
                            select new
                            {
                                user.CEmployeeName,
                                travel.CApplyDate,
                                travel.CEmployeeId,
                                travel.CAmont,
                                travel.CCheckStatus,
                                travel.CTravelStartTime,
                                travel.CTravelEndTime,
                                travel.CReason,
                                travel.CApplyNumber
                            };

                foreach (var item in table)
                {
                    Travel_Expense_ApplicationViewModel traObj = new Travel_Expense_ApplicationViewModel()
                    {
                        employeeName = item.CEmployeeName,
                        CApplyNumber = item.CApplyNumber,
                        CApplyDate = item.CApplyDate,
                        CTravelStartTime = item.CTravelStartTime,
                        CTravelEndTime = item.CTravelEndTime,
                        CAmont = item.CAmont,
                        CReason = item.CReason,
                        CEmployeeId = item.CEmployeeId,
                        CCheckStatus = item.CCheckStatus
                    };
                    list.Add(traObj);
                }
            }
            return View(list);
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
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(c => c.CApplyNumber == i);
                if (travel.CCheckStatus == 1)
                {
                    if (travel != null)
                    {
                        travel.CCheckStatus = 2;
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
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(c => c.CApplyNumber == i);
                if (travel.CCheckStatus == 1)
                {
                    if (travel != null)
                    {
                        travel.CCheckStatus = 3;
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
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == id);
                if (travel != null)
                {
                    //if (leave.CCheckStatus==1)
                    {
                        travel.CCheckStatus = 2;
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
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == id);
                if (travel != null)
                {
                    //if (leave.CCheckStatus==1)
                    {
                        travel.CCheckStatus = 3;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}

  

