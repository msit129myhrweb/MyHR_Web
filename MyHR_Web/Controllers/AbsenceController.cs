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
        #region 上下班打卡
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

        public JsonResult getClockString_on(string c)//上班
        {
            string[] ids = c.Split('\"', '"', ',',' ');
            var id=ids[1];
            var date = ids[2];
            var time = ids[4];
            var on = date + " " + time;
            dbMyCompanyContext db = new dbMyCompanyContext();

            TAbsence a = db.TAbsences.FirstOrDefault(z => z.CEmployeeId == int.Parse(id) && z.COn.Value.Date == DateTime.Today);
            if (a==null)//今天已打卡
            {
                TAbsence b = new TAbsence()
                {
                    CEmployeeId = int.Parse(id),
                    COn = DateTime.Parse(on),
                };
                db.TAbsences.Add(b);
                db.SaveChanges();
            }
            return Json(c);
        }
        public JsonResult getClockString_off(string c)//下班
        {
            string[] ids = c.Split('\"', '"', ',', ' ');
            var id = ids[1];
            var date = ids[2];
            var time = ids[4];

            string[] ymd = date.Split('-');
            var year = ymd[0];
            var month = ymd[1];
            var day = ymd[2];

            var on = date + " " + time;
            dbMyCompanyContext db = new dbMyCompanyContext();
            TAbsence a = db.TAbsences.FirstOrDefault(z => z.CEmployeeId == int.Parse(id) && z.COn.Value.Date == DateTime.Today);

            if (a!=null)//有打上班卡
            {
                if (a.COn.Value.Month.ToString().Length < 2)
                {
                    string addMonth = "0" + a.COn.Value.Month.ToString();
                    a = db.TAbsences.FirstOrDefault(x => x.CEmployeeId == int.Parse(id) && x.COn.Value.Year.ToString() == year && addMonth == month && x.COn.Value.Day.ToString() == day);
                }
                else
                    a = db.TAbsences.FirstOrDefault(x => x.CEmployeeId == int.Parse(id) && x.COn.Value.Year.ToString() == year && a.COn.Value.Month.ToString() == month && x.COn.Value.Day.ToString() == day);

                if (a.COn.Value.Month.ToString().Length < 2)
                {
                    string addMonth = "0" + a.COn.Value.Month.ToString();

                    string ON = a.COn.Value.Year.ToString() + "-" + addMonth + "-" + a.COn.Value.Day.ToString();
                    if (date == ON)
                    {
                        a.COff = DateTime.Parse(on);
                        db.SaveChanges();
                    }
                }
                else
                {
                    string ON = a.COn.Value.Year.ToString() + "-" + a.COn.Value.Month.ToString() + "-" + a.COn.Value.Day.ToString();
                    if (date == ON)
                    {
                        a.COff = DateTime.Parse(on);
                        db.SaveChanges();
                    }
                }
            }
            else//沒打上班卡
            {
                TAbsence b = new TAbsence()
                {
                    CEmployeeId = int.Parse(id),
                    COff = DateTime.Parse(on)
                };
                db.TAbsences.Add(b);
                db.SaveChanges();
            }
            return Json(c);
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
