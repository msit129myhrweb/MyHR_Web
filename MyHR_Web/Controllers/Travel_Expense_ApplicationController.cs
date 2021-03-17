using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;


namespace MyHR_Web.Controllers
{
    public class Travel_Expense_ApplicationController : Controller
    {
        public IActionResult List()
        {
            // keyword searching
            string keyword = Request.Query["txtKeyword"];
            IEnumerable<TTravelExpenseApplication> table = null;
            if (string.IsNullOrEmpty(keyword))
                table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications
                        //where travel.CDepartmentId.ToString()== ViewData[prjCoreDemo.ViewModel.CDictionary.CURRENT_LOGINED_USERDEPARTMENT].ToString()
                        select travel;
            else
                table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications
                        where travel.CAmont.ToString().Contains(keyword) ||
                              travel.CReason.Contains(keyword) ||
                              travel.CApplyNumber.ToString().Contains(keyword)
                        select travel;

            List < CTravelViewModel > list = new List<CTravelViewModel>();
            foreach (TTravelExpenseApplication item in table)
                list.Add(new CTravelViewModel(item));

            return View(list);
        }
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

    }
}
