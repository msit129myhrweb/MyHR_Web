using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany_.NetCore_Janna.Controllers
{
    public class SalaryController : Controller
    {
        dbMyCompanyContext MyHR = new dbMyCompanyContext();

        public IActionResult SalaryList()
        {

            int UserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));
             var table = MyHR.TUsers
              .Include(c => c.CDepartment)
              .Include(c => c.CJobTitle)
              .Where(c => c.CEmployeeId == UserID)
              .Select(c=>new CSalaryViewModel
            {
                CEmployeeName = c.CEmployeeName,
                CDepartment = c.CDepartment.CDepartment,
                CEmployeeId = c.CEmployeeId,
                CJobTitle = c.CJobTitle.CJobTitle,
                CJobTitleSalary = c.CJobTitle.CJobTitleSalary

            });
            List<CSalaryViewModel> T = new List<CSalaryViewModel>();

            foreach (var item in table)
            {
                CSalaryViewModel obj = new CSalaryViewModel()
                {
                    CEmployeeName = item.CEmployeeName,
                    CDepartment = item.CDepartment,
                    CEmployeeId = item.CEmployeeId,
                    CJobTitle = item.CJobTitle,
                    CJobTitleSalary = item.CJobTitleSalary

                };
                T.Add(obj);
            }

            return View(table.ToList());
        }


        public IActionResult ConfirmPassword()
        {
            ViewBag.Name = HttpContext.Session.GetString("CURRENT_LOGINED_USERENNAME");
            return PartialView();
        }
        [HttpPost]
        public IActionResult ConfirmPassword(CSalaryLoginViewModel p)
        {
            string Password1 = Request.Form["Password"].ToString();
            if (string.IsNullOrEmpty(Password1))
            {
                return RedirectToAction("ConfirmPassword");
            }
            else
            {
                using (dbMyCompanyContext MyHR = new dbMyCompanyContext())
                {
                    int CurrentUserID = int.Parse(HttpContext.Session.GetString("CURRENT_LOGINED_USERID"));

                    var user = MyHR.TUsers.AsEnumerable().FirstOrDefault(x => int.Parse(x.CPassWord) == int.Parse(Password1) && x.CEmployeeId == CurrentUserID);

                    if (user != null)
                    {
                        return RedirectToAction("SalaryList");
                    }
                    else
                    {
                        return RedirectToAction("ConfirmPassword");
                    }
                }
            }
        }

        




    }
}
