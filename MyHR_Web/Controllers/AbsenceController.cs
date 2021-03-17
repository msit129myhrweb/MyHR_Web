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
        DateTime now = DateTime.Now; //現在時間
        DateTime off = DateTime.Today.AddHours(18); //下班時間18:00
        DateTime on = DateTime.Today.AddHours(9); //上班時間9:00

        
        public string Clock()//打卡
        {
            var t = from clock in (new dbMyCompanyContext()).TAbsences
                    let x = DateTime.Today.ToString("yyyy-MM-dd")
                    where clock.CEmployeeId.ToString() == ViewData[CDictionary.CURRENT_LOGINED_USERID].ToString()
                    select clock;

            if (t.ToList().Count==0)
            {

            }
            return "";
        }
        public string showClockStatus()
        {
            if (now>on)
            {

            }
            return "";
        }
        public IActionResult List()
        {
            string status = "";
            
            {

            }
            ViewData["clockStatus"]=status;
            var table = from absence in (new dbMyCompanyContext()).TAbsences
                        select absence;
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (TAbsence item in table)
                list.Add(new CAbsenceViewModel(item));

            return View(list);
        }
        public IActionResult Create()//上下班皆未打卡
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

            if (string.IsNullOrEmpty(a.COn.ToString("yyyy-MM-dd")) || string.IsNullOrEmpty(a.COff.ToString("yyyy-MM-dd")))
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
        public IActionResult Edit(int? userId)//上班未下班未打卡
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
