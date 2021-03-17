using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Controllers
{
    public class LeaveApplicationController : Controller
    {
        public IActionResult List()
        {
            // keyword searching
            var table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                        select leave;
            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            foreach (TLeaveApplication item in table)
                list.Add(new TLeaveApplicationViewModel(item));

            return View(list) ;
        }
        public IActionResult Edit(int? cappyNum)
        {
            if (cappyNum!=null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(l=>l.CApplyNumber == cappyNum);
                if (leave!=null)
                {
                    return View(new TLeaveApplicationViewModel(leave));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(TLeaveApplicationViewModel leaveEdit)
        {
            if (leaveEdit!=null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave_Edited = db.TLeaveApplications.FirstOrDefault(l => l.CApplyNumber == leaveEdit.CApplyNumber);
                if (leave_Edited!=null)
                {
                    leave_Edited.CCheckStatus = leaveEdit.CCheckStatus;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
