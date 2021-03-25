using Microsoft.AspNetCore.Mvc;

using MyHR_Web.ViewModel;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class TravelController : Controller
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        public JsonResult tcheckStatus()
        {
            var tchecks = from t in db.TCheckStatuses
                       select t;
            return Json(tchecks);
        }
        public IActionResult List()
        {
            var traveltable = from t in db.TTravelExpenseApplications
                              join c in db.TCheckStatuses on t.CCheckStatus equals c.CCheckStatusId
                              select new
                              {
                                  CApplyNumber = t.CApplyNumber,
                                  CDepartmentId = t.CDepartmentId,
                                  CEmployeeId = t.CEmployeeId,
                                  CReason = t.CReason,
                                  CApplyDate = t.CApplyDate,
                                  CTravelStartTime = t.CTravelStartTime,
                                  CTravelEndTime = t.CTravelEndTime,
                                  CAmont = t.CAmont,
                                  CCheckStatus = c.CCheckStatusId
                              };
            List<CTravelListViewModel> tlist = new List<CTravelListViewModel>();
           foreach(var titem in traveltable)
            {
                CTravelListViewModel ctlvm = new CTravelListViewModel()
                {
                    CApplyNumber = titem.CApplyNumber,
                    CDepartmentId = titem.CDepartmentId,
                    CEmployeeId = titem.CEmployeeId,
                    CReason = titem.CReason,
                    CApplyDate = titem.CApplyDate,
                    CTravelStartTime = titem.CTravelStartTime,
                    CTravelEndTime = titem.CTravelEndTime,
                    CAmont = titem.CAmont,
                    CCheckStatus = titem.CCheckStatus
                };
                tlist.Add(ctlvm);
            }
            return View(tlist);  
        }
        public IActionResult Create()
        {
            //string a = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            //ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            //string b = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            //ViewData[CDictionary.LOGIN_USERID] = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            return View(new CTravelViewModel 
            {
                CDepartmentName = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT),
                CEmployeeId = int.Parse(HttpContext.Session.GetString(CDictionary.LOGIN_USERID))
            });
        }
        [HttpPost]
        public IActionResult Create(CTravelViewModel t)
        {
            if(ModelState.IsValid == false)
            {
                return View(t);
            }
            var depart = db.TUserDepartments.FirstOrDefault(e => e.CDepartment == t.CDepartmentName);
            //TODO 部門不存在
            if(depart == null)
            {
                throw new NotImplementedException();
            }
            t.CDepartmentId = depart.CDepartmentId;
            db.TTravelExpenseApplications.Add(t.travel);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                TTravelExpenseApplication d = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == id);

                if (d != null)
                {
                    db.TTravelExpenseApplications.Remove(d);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            { 
                TTravelExpenseApplication t = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == id);
                TCheckStatus ch = db.TCheckStatuses.FirstOrDefault(ch => ch.CCheckStatusId == t.CCheckStatus);
                if (t != null)
                {
                    return View(new CTravelViewModel(t,ch));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Edit(CTravelViewModel t_travelEdit)
        {
            if (t_travelEdit != null)
            {
                TTravelExpenseApplication l_tea被修改 = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == t_travelEdit.CApplyNumber);
                TCheckStatus l_tcs被修改=db.TCheckStatuses.FirstOrDefault(t => t.CCheckStatusId == (int)t_travelEdit.CCheckStatus);
                if (l_tea被修改 != null)
                {
                    l_tea被修改.CEmployeeId = t_travelEdit.CEmployeeId;
                    l_tea被修改.CDepartmentId = t_travelEdit.CDepartmentId;
                    l_tea被修改.CReason = t_travelEdit.CReason;
                    l_tea被修改.CApplyDate = t_travelEdit.CApplyDate;
                    l_tea被修改.CTravelStartTime = t_travelEdit.CTravelStartTime;
                    l_tea被修改.CTravelEndTime = t_travelEdit.CTravelEndTime;
                    l_tea被修改.CAmont = t_travelEdit.CAmont;
                    l_tcs被修改.CCheckStatusId = (int)t_travelEdit.CCheckStatus;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
