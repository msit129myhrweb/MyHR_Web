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
    public class Travel_Expense_ApplicationController : Controller
    {

        public IActionResult List()
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            IEnumerable<TTravelExpenseApplication> table = null;

            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string Keyword = Request.Form["txtKeyword"];
                string ApplyNum = Request.Form["txtApplyNum"];

                if (!string.IsNullOrEmpty(Keyword))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications
                            where travel.CDepartmentId == DepId && travel.CReason.Contains(Keyword)
                            select travel;
                }

                else if(!string.IsNullOrEmpty(ApplyNum))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications
                            where travel.CDepartmentId == DepId && travel.CApplyNumber.ToString().Contains(Keyword)
                            select travel;
                }
                else if (string.IsNullOrEmpty(Keyword)&&string.IsNullOrEmpty(ApplyNum))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            where travel.CDepartmentId == DepId
                            select travel;
                }

                List<CTravelViewModel> list = new List<CTravelViewModel>();
                foreach (TTravelExpenseApplication item in table)
                    list.Add(new CTravelViewModel(item));

                return View(list);

            }

            return RedirectToAction("List");

        }

        #region Edit
        //通過或退件
        public IActionResult Edit(int? capplyNum)
        {
            if (capplyNum != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == capplyNum);
                if (travel != null)
                {
                    return View(new CTravelViewModel(travel));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(CTravelViewModel travelEdit)
        {
            if (travelEdit != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication travel_Edited = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == travelEdit.CApplyNumber);
                if (travel_Edited != null)
                {
                    travel_Edited.CCheckStatus = travelEdit.CCheckStatus;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        #endregion
    }
}
