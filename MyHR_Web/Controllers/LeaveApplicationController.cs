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
        dbMyCompanyContext DB = new dbMyCompanyContext();

        public IActionResult List()
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            List<TLeave> leaveCate = getLeaveCategory();//取得資料庫的類別
            ViewBag.leaveCategory = leaveCate;

            List<TCheckStatus> checkSta = getCheckStatus();//取的資料庫的審核狀態
            ViewBag.leaveStatus = checkSta;

            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string AppNum = Request.Form["txtAppNum"];
                string Id = Request.Form["txtId"];
                string Name = Request.Form["txtName"];

                //AppNum有值
                if (!string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                            join user in DB.TUsers on leave.CEmployeeId equals user.CEmployeeId
                            where leave.CDepartmentId == DepId &&
                                  leave.CApplyNumber.ToString().Contains(AppNum)
                            orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }

                }
                //Id有值
                else if (string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      leave.CEmployeeId.ToString().Contains(Id)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }

                }
                //Name有值
                else if (string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      user.CEmployeeName.Contains(Name)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }

                }
                
                //AppNum, Id有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      leave.CApplyNumber.ToString().Contains(AppNum)&&
                                      leave.CEmployeeId.ToString().Contains(Id)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }
                }
                //AppNum, Name有值
                else if (!string.IsNullOrEmpty(AppNum) && string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      leave.CApplyNumber.ToString().Contains(AppNum)&&
                                      user.CEmployeeName.Contains(Name)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }
                }
                //Id, Name有值
                else if (string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      leave.CEmployeeId.ToString().Contains(Id)&&
                                      user.CEmployeeName.Contains(Name)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }
                }

                //AppNum, Id, Name有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name))
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId &&
                                      leave.CApplyNumber.ToString().Contains(AppNum) &&
                                      leave.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }
                }

                //全部皆為空白
                else if (AppNum=="" && Id=="" && Name=="")
                {
                    var table = from leave in DB.TLeaveApplications
                                join user in DB.TUsers
                                on leave.CEmployeeId equals user.CEmployeeId
                                where leave.CDepartmentId == DepId
                                orderby leave.CApplyDate descending
                                select new
                                {
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
                            CEmployeeId = item.CEmployeeId,
                            CReason = item.CReason,
                            CLeaveStartTime = item.CLeaveStartTime,
                            CLeaveEndTime = item.CLeaveEndTime,
                            CLeaveCategory = item.CLeaveCategory,
                            CCheckStatus = item.CCheckStatus

                        };
                        list.Add(newObj);
                    }
                }
            }
            //全部皆為空白
            else
            {
                var table = from leave in DB.TLeaveApplications
                            join user in DB.TUsers
                            on leave.CEmployeeId equals user.CEmployeeId
                            where leave.CDepartmentId == DepId
                            orderby leave.CApplyDate descending
                            select new
                            {
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
        private List<TLeave> getLeaveCategory()//取得資料庫請假類別
        {
            try
            {
                List<TLeave> list = new List<TLeave>();
                list = (from l in DB.TLeaves
                        select l).ToList();
                return list;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return null;
            }
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
                string err= ex.ToString();
                return null;
            }
        }

        public IActionResult date_search(int? cate, int? status, DateTime? start, DateTime? end)
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));

            var table = DB.TLeaveApplications.Join(DB.TUsers,
                l => l.CEmployeeId,
                u => u.CEmployeeId,
                (l, u) => new {
                    CEmployeeId = l.CEmployeeId,
                    CApplyNumber = l.CApplyNumber,
                    CApplyDate = l.CApplyDate,
                    CLeaveCategory = l.CLeaveCategory,
                    CLeaveStartTime = l.CLeaveStartTime,
                    CLeaveEndTime = l.CLeaveEndTime,
                    CCheckStatus = l.CCheckStatus,
                    CReason = l.CReason,
                    employeeName = u.CEmployeeName,
                    CDepartmentId=u.CDepartmentId
                }).OrderByDescending(l => l.CApplyDate).AsEnumerable().Where(sc=>
                sc.CDepartmentId==DepId &&
                (cate!=null?sc.CLeaveCategory==cate:true)&&
                (status!=null?sc.CCheckStatus==status:true)&&
                (start!=null? DateTime.Parse(sc.CApplyDate)>=start:true)&&
                (end!=null? DateTime.Parse(sc.CApplyDate)<=end:true)).ToList();

            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            foreach (var item in table)
            {
                TLeaveApplicationViewModel leaObj = new TLeaveApplicationViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CEmployeeId = item.CEmployeeId,
                    employeeName=item.employeeName,
                    CApplyDate = item.CApplyDate,
                    CLeaveCategory = item.CLeaveCategory,
                    CLeaveStartTime = item.CLeaveStartTime,
                    CLeaveEndTime = item.CLeaveEndTime,
                    CReason = item.CReason,
                    CCheckStatus = item.CCheckStatus
                };
                list.Add(leaObj);
            }
            return PartialView("date_search",list);
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
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(c => c.CApplyNumber == i);
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
                    if (leave.CCheckStatus == 1)
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
