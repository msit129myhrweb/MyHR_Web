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
            IEnumerable<TLeaveApplication> table = null;

            // keyword searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string Keyword = Request.Form["txtKeyword"];

                //事由跟申請單號為空
                if (string.IsNullOrEmpty(Keyword))
                {
                    table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                            where leave.CDepartmentId == DepId
                            select leave;
                }
                //事由、申請單號都(或其一)有值
                else
                {
                    table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                            //join category in (new dbMyCompanyContext()).TLeaves on leave.CLeaveCategory equals category.CLeaveId
                            where leave.CDepartmentId == DepId &&
                                  leave.CReason.Contains(Keyword) ||
                                  leave.CApplyNumber.ToString().Contains(Keyword)/* ||*/
                                  //category.CLeaveCategory.Contains(Keyword)
                            select leave;
                }
            }
            //事由跟申請單號為空白
            else
            {
                table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                        where leave.CDepartmentId == DepId
                        select leave;
            }
            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            foreach (TLeaveApplication item in table)
                list.Add(new TLeaveApplicationViewModel(item));

            return View(list) ;
        }

        #region Edit
        //通過或退件
        public IActionResult Edit(int? capplyNum)
        {
            if (capplyNum != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLeaveApplication leave = db.TLeaveApplications.FirstOrDefault(l=>l.CApplyNumber == capplyNum);
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
        #endregion
    }
}
