using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany_.NetCore_Janna.Controllers
{
    public class SalaryController : Controller
    {
        public IActionResult SalaryList()
        {
            return View();
        }
        
        
        public IActionResult ConfirmPassword()
        {
            return PartialView();
        }



        [HttpPost]
        public IActionResult ConfirmPassword(CLoginViewModel p)
        {
            TUser user = (new dbMyCompanyContext()).TUsers.FirstOrDefault(c =>
             c.CEmployeeId.Equals(p.txtAccount) && c.CPassWord.Equals(p.txtPassword));

            if (user != null)
            {
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, user.CEmployeeName);
                return RedirectToAction("Index");
            }

            return View();

        }
    }
}
