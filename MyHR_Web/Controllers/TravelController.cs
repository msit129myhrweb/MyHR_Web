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
using MyHR_Web.MyClass;

namespace MyHR_Web.Controllers
{
    
    public class TravelController : BaseController
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        List<CTravelViewModel> tlist = new List<CTravelViewModel>();

        public IActionResult List(DateTime? startDate = null,DateTime? endDate = null)
        {
            ViewBag.StartDate = startDate;   //時間查詢
            ViewBag.EndDate = endDate;
            ViewBag.Status = db.TCheckStatuses.ToList();
            IQueryable<TTravelExpenseApplication> tt = db.TTravelExpenseApplications.AsQueryable();

            if(startDate.HasValue)          //時間有值
            {
                tt = tt.Where(e => e.CApplyDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                tt = tt.Where(e => e.CApplyDate < endDate.Value);
            }

            if (getUserDepartmentId() == 6)     //設觀看權限顯示畫面
            {
                var traveltable = from t in tt
                                  join c in db.TCheckStatuses on t.CCheckStatus equals c.CCheckStatusId
                                  join u in db.TUsers on t.CEmployeeId equals u.CEmployeeId
                                  select new
                                  {
                                      CApplyNumber = t.CApplyNumber,
                                      CDepartmentId = t.CDepartmentId,
                                      CEmployeeId = t.CEmployeeId,
                                      CEmployeeName = u.CEmployeeName,
                                      CReason = t.CReason,
                                      CApplyDate = t.CApplyDate,
                                      CTravelStartTime = t.CTravelStartTime,
                                      CTravelEndTime = t.CTravelEndTime,
                                      CAmont = t.CAmont,
                                      CCheckStatus = c.CCheckStatusId
                                  };
                foreach (var titem in traveltable)
                {
                    CTravelViewModel ctlvm = new CTravelViewModel()
                    {
                        CApplyNumber = titem.CApplyNumber,
                        CDepartmentId = titem.CDepartmentId,
                        CEmployeeId = titem.CEmployeeId,
                        CEmployeeName =titem.CEmployeeName,
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
            else
            {
                var traveltable = from t in tt
                                  join c in db.TCheckStatuses on t.CCheckStatus equals c.CCheckStatusId
                                  join u in db.TUsers on t.CEmployeeId equals u.CEmployeeId
                                  where t.CEmployeeId == getUserId()
                                  select new
                                  {
                                      CApplyNumber = t.CApplyNumber,
                                      CDepartmentId = t.CDepartmentId,
                                      CEmployeeId = t.CEmployeeId,
                                      CEmployeeName = u.CEmployeeName,
                                      CReason = t.CReason,
                                      CApplyDate = t.CApplyDate,
                                      CTravelStartTime = t.CTravelStartTime,
                                      CTravelEndTime = t.CTravelEndTime,
                                      CAmont = t.CAmont,
                                      CCheckStatus = c.CCheckStatusId
                                  };
               
                foreach (var titem in traveltable)
                {
                    CTravelViewModel ctlvm = new CTravelViewModel()
                    {
                        CApplyNumber = titem.CApplyNumber,
                        CDepartmentId = titem.CDepartmentId,
                        CEmployeeId = titem.CEmployeeId,
                        CEmployeeName = titem.CEmployeeName,
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
        }
        public IActionResult Create()
        {
            ViewBag.Departments = db.TUserDepartments.ToList();     
            ViewBag.Status = db.TCheckStatuses.ToList();
            DateTime now = DateTime.UtcNow.AddHours(8).Date;
            return View(new CTravelViewModel
            {
                CEmployeeName = getUserName(),
                CDepartmentId = getUserDepartmentId(),
                CEmployeeId = getUserId(),
                CTravelEndTime = now,
                CTravelStartTime = now,
                CApplyDate = now
            });
        }
        [HttpPost]
        public IActionResult Create(CTravelViewModel model)
        {
            if(ModelState.IsValid == false)
            {
                ViewBag.Departments = db.TUserDepartments.ToList();
                ViewBag.Status = db.TCheckStatuses.ToList();
                return View(model);
            }
            
            db.TTravelExpenseApplications.Add(new TTravelExpenseApplication 
            {
                CAmont = model.CAmont,
                CApplyDate = DateTime.UtcNow.AddHours(8),
                CCheckStatus = model.CCheckStatus,
                CDepartmentId = model.CDepartmentId,
                CEmployeeId = model.CEmployeeId,
                CReason = model.CReason,
                CTravelEndTime = model.CTravelEndTime,
                CTravelStartTime = model.CTravelStartTime
            });
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
            if(id.HasValue == false)
            {
                return RedirectToAction("List");
            }

            TTravelExpenseApplication et = db.TTravelExpenseApplications.Where(e => e.CApplyNumber == id).FirstOrDefault();

            if(et == null)
            {
                return RedirectToAction("List");
            }

            var result = new CTravelViewModel 
            { 
                CAmont = et.CAmont,
                CApplyNumber = et.CApplyNumber,
                CCheckStatus = et.CCheckStatus,
                CDepartmentId = et.CDepartmentId,
                CEmployeeId = et.CEmployeeId,
                CReason = et.CReason,
                CTravelEndTime = et.CTravelEndTime,
                CTravelStartTime = et.CTravelStartTime
            };

            ViewBag.Departments = db.TUserDepartments.ToList();
            ViewBag.Status = db.TCheckStatuses.ToList();
            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(CTravelViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Departments = db.TUserDepartments.ToList();
                ViewBag.Status = db.TCheckStatuses.ToList();
                
                return View(model);
            }
        
            var entity = db.TTravelExpenseApplications.Where(e => e.CApplyNumber == model.CApplyNumber).FirstOrDefault();

            if(entity == null)
            {
                return RedirectToAction("List");
            }

            entity.CAmont = model.CAmont;
            entity.CReason = model.CReason;
            entity.CTravelEndTime = model.CTravelEndTime;
            entity.CTravelStartTime = model.CTravelStartTime;
            entity.CCheckStatus = model.CCheckStatus;
          
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
