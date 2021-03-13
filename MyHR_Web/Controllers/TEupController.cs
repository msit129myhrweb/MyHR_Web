using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Models
{
    public class TEupController : Controller
    {
        public IActionResult ListAll()
        {
            var travaletable = from r in (new dbMyCompanyContext()).TTravelExpenseApplications
                               select r;
            return View(travaletable);
        }
        public IActionResult Edit(int? cApplyNumber)
        {
            TTravelExpenseApplication x = null;
            if(cApplyNumber != null)
            {
                x = (new CTravelFactory()).getById((int)cApplyNumber);
            }
            return View(x);
        }
        [HttpPost]
        public ActionResult Edit(TTravelExpenseApplication t)       
        {                                           
            (new CTravelFactory()).update(t);     
            return RedirectToAction("List");        
        }
    }
}
