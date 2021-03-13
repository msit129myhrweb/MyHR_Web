using Microsoft.AspNetCore.Mvc;

using MyHR_Web.ViewModel;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class TravelController : Controller
    {
        //public JsonResult loadtravelcbb()
        //{
        //    var check = from t in (new dbMyCompanyContext()).TTravelExpenseApplications

        //                join c in (new dbMyCompanyContext()).TCheckStatuses on t.CCheckStatus equals c.CCheckStatusId
        //                select new
        //                {
        //                    checktest = c.CCheckStatus
        //                };
        //    return Json(check);           
        //}
        public IActionResult List()
        {
            var table = from t in (new dbMyCompanyContext()).TTravelExpenseApplications
                        select t;
            List<CTravelViewModel> list = new List<CTravelViewModel>();
            foreach (TTravelExpenseApplication p in table)
                list.Add(new CTravelViewModel(p));
            return View(list);
            
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CTravelViewModel t)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TTravelExpenseApplications.Add(t.travel);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
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
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication d = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == id);

                if (d != null)
                {
                    return View(new CTravelViewModel(d));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Edit(CTravelViewModel t_travelEdit)
        {
            if (t_travelEdit != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication l_product被修改 = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == t_travelEdit.CApplyNumber);

                if (l_product被修改 != null)
                {
                    l_product被修改.CEmployeeId = t_travelEdit.CEmployeeId;
                    l_product被修改.CDepartmentId = t_travelEdit.CDepartmentId;
                    l_product被修改.CReason = t_travelEdit.CReason;
                    l_product被修改.CApplyDate = t_travelEdit.CApplyDate;
                    l_product被修改.CTravelStartTime = t_travelEdit.CTravelStartTime;
                    l_product被修改.CTravelEndTime = t_travelEdit.CTravelEndTime;
                    l_product被修改.CAmont = t_travelEdit.CAmont;
                    l_product被修改.CCheckStatus = t_travelEdit.CCheckStatus;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
