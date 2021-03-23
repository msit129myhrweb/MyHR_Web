using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        #region List
        public IActionResult List()
        {
            string status = "";
            {

            }

            ViewData["clockStatus"]=status;

            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var table = from absence in (new dbMyCompanyContext()).TAbsences
                        where absence.CEmployeeId== userId
                        select absence;
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (TAbsence item in table)
                list.Add(new CAbsenceViewModel(item));

            return View(list);
        }
        #endregion

        #region Create
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
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));


            if (string.IsNullOrEmpty(a.COn.ToString("yyyy-MM-dd")) || string.IsNullOrEmpty(a.COff.ToString("yyyy-MM-dd")))
            {
                return View();
            }
            else
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                db.TAbsences.Add(a.absence);
               // db.SaveChanges();
                return RedirectToAction("List");
            }

           // ModelState.IsValid;

        }
        #endregion

        #region Edit
        public IActionResult Edit(int? userId)//上班或下班未打卡
        {
            if (userId!=null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TAbsence absence = db.TAbsences.FirstOrDefault(a => a.CEmployeeId == userId);
                if (absence!=null)
                {
                    return View(new CAbsenceViewModel(absence));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(CAbsenceViewModel absenceEdit)
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            if (absenceEdit!=null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TAbsence absenceEdited = db.TAbsences.FirstOrDefault(a => a.CEmployeeId == absenceEdit.CEmployeeId);

                if (absenceEdited!=null)
                {
                    absenceEdited.COn = absenceEdit.COn;
                    absenceEdited.COff = absenceEdit.COff;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        #endregion

        DateTime now = DateTime.Now; //現在時間
        DateTime off = DateTime.Today.AddHours(18); //下班時間18:00
        DateTime on = DateTime.Today.AddHours(9); //上班時間9:00




        public string Clock()//打卡
        {
            var t = from clock in (new dbMyCompanyContext()).TAbsences
                    let x = DateTime.Today.ToString("yyyy-MM-dd")
                    where clock.CEmployeeId.ToString() == ViewData[CDictionary.CURRENT_LOGINED_USERID].ToString()
                    select clock;

            if (t.ToList().Count == 0)
            {

            }
            return "";
        }
        public string showClockStatus()
        {
            if (now > on)
            {

            }
            return "";
        }
    }
}
