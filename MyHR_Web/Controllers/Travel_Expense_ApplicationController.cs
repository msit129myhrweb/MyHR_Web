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
        dbMyCompanyContext DB = new dbMyCompanyContext();

        public IActionResult List()
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            List<Travel_Expense_ApplicationViewModel> list = new List<Travel_Expense_ApplicationViewModel>();
           
            List<TCheckStatus> checkSta = getCheckStatus();
            ViewBag.travelStatus = checkSta;


            //searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string AppNum = Request.Form["txtAppNum"];
                string Id = Request.Form["txtId"];
                string Name = Request.Form["txtName"];

                //AppNum有值
                if (!string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CApplyNumber.ToString().Contains(AppNum)
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
                //Id有值
                else if (string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CEmployeeId.ToString().Contains(Id)
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
                //Name有值
                else if (string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      user.CEmployeeName.Contains(Name)
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
                //AppNum, Id有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id)
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
                //Name, Id有值
                else if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(AppNum))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
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
                //Name, AppNum有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Id))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      user.CEmployeeName.Contains(Name)
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
                //全部有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
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
                //全部條件為空白
                else if (AppNum==""&& Id==""&& Name=="")
                {
                    var table = from travel in DB.TTravelExpenseApplications
                                join user in DB.TUsers on travel.CEmployeeId equals user.CEmployeeId
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

        private List<TCheckStatus> getCheckStatus()//取得資料庫審核狀態
        {
            try
            {
                List<TCheckStatus> list = new List<TCheckStatus>();
                list = (from c in DB.TCheckStatuses
                        select c).ToList();
                return list;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return null;
            }
        }

        public IActionResult dateSearch(int? status, DateTime? start, DateTime? end)
        {
            return PartialView();
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
                if (travel != null)
                {
                    if (travel.CCheckStatus == 1)
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
                if (travel != null)
                {
                    if (travel.CCheckStatus == 1)
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
                    if (travel.CCheckStatus==1)
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
                    if (travel.CCheckStatus==1)
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

  

