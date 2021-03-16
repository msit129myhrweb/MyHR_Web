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
    public class AbsenceController : Controller
    {
        public IActionResult List()
        {
            var table = from absence in (new dbMyCompanyContext()).TAbsences
                        select absence;
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (TAbsence item in table)
                list.Add(new CAbsenceViewModel(item));

            return View(list);
        }
        public IActionResult Create()
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
return View();
        }

        [HttpPost]
        public IActionResult Create(CAbsenceViewModel a)
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);

            if (string.IsNullOrEmpty(a.COn.ToString("yyyy-MM-dd"))||string.IsNullOrEmpty(a.COff.ToString("yyyy-MM-dd")))
            {
                return View();
            }
            else
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                db.TAbsences.Add(a.absence);
                db.SaveChanges();
                return RedirectToAction("List");

            }

        }
    }
}
