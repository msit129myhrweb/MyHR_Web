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

            //searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string Keyword = Request.Form["txtKeyword"];

                //事由跟申請單號為空白
                if (string.IsNullOrEmpty(Keyword))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            where travel.CDepartmentId == DepId
                            select travel;
                }
                //事由跟申請單號都(或其一)有值
                else
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            where travel.CDepartmentId == DepId && 
                                  travel.CReason.Contains(Keyword) || 
                                  travel.CApplyNumber.ToString().Contains(Keyword)
                            select travel;
                }
            }
            //事由跟申請單號為空白
            else
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
