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
        dbMyCompanyContext db = new dbMyCompanyContext();
        DateTime Coff = DateTime.Today.AddHours(18); //下班時間18:00
        DateTime Con = DateTime.Today.AddHours(9); //上班時間9:00
        DateTime now = DateTime.Now;
        #region 上下班打卡
        public IActionResult List()
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = from t in db.TAbsences.AsEnumerable()
                       where t.CEmployeeId == userId
                       select t;

            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in time)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber=item.CApplyNumber,
                    COn=item.COn,
                    COff=item.COff,
                    CSatus=item.CStatus
                };
                list.Add(avm);
            }
            return View(list);
        }

        public JsonResult getClockString_on(string c)//上班
        {
            string[] ids = c.Split('\"', '"', ',',' ');
            var id=ids[1];
            var date = ids[2];
            var time = ids[4];
            var on = date + " " + time;

            TAbsence a = db.TAbsences.FirstOrDefault(z => z.COn.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id));
            TAbsence ab = db.TAbsences.FirstOrDefault(z => z.COff.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id));
            if (a==null && ab==null)//今天未打卡
            {
                if (now < Con)
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = int.Parse(id),
                        COn = DateTime.Parse(on),
                        CStatus = "準時"
                    };
                    db.TAbsences.Add(b);
                }
                else if (now > Con)
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = int.Parse(id),
                        COn = DateTime.Parse(on),
                        CStatus = "遲到"
                    };
                    db.TAbsences.Add(b);
                }
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
            TAbsence a = db.TAbsences.FirstOrDefault(z => z.COn.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id));
            TAbsence ab = db.TAbsences.FirstOrDefault(z => z.COn.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id));

            if (a!=null)//有打上班卡
            {
                string y = a.COn.Value.Year.ToString();
                string m = a.COn.Value.Month.ToString();
                string d = a.COn.Value.Day.ToString();
                double aonTime = a.COn.Value.TimeOfDay.TotalSeconds;//上班卡的時間(秒數)
                double ConTime = Con.TimeOfDay.TotalSeconds;//九點(秒數)

                if (m.Length < 2 && d.Length <2)
                {
                     m = "0" + a.COn.Value.Month.ToString();
                     y = a.COn.Value.Year.ToString();
                     d = "0" + a.COn.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            a.CStatus = "遲到";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            a.CStatus = "準時";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length < 2 && d.Length > 2)
                {
                    m = "0" + a.COn.Value.Month.ToString();
                    y = a.COn.Value.Year.ToString();
                    d = a.COn.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            a.CStatus = "遲到";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            a.CStatus = "準時";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length > 2 && d.Length < 2)
                {
                    m = a.COn.Value.Month.ToString();
                    y = a.COn.Value.Year.ToString();
                    d = "0" + a.COn.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            a.CStatus = "遲到";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            a.CStatus = "準時";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length > 2 && d.Length > 2)
                {
                    m = a.COn.Value.Month.ToString();
                    y = a.COn.Value.Year.ToString();
                    d = a.COn.Value.Day.ToString();

                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            a.CStatus = "遲到";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            a.CStatus = "準時";
                            a.COff = DateTime.Parse(on);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else if (a==null && ab==null)//沒打上班卡
            {
                TAbsence b = new TAbsence()
                {
                    CEmployeeId = int.Parse(id),
                    COff = DateTime.Parse(on),
                    CStatus ="異常"
                };
                db.TAbsences.Add(b);
                db.SaveChanges();
            }
            return Json(c);
        }
        #endregion

        #region Edit打卡補登
       
        public IActionResult Edit(int? applyNum/*, int id, DateTime date, int when*/)
        {
            if (applyNum != null)
            {
                TAbsence ab_on = db.TAbsences.FirstOrDefault(a => a.CApplyNumber == applyNum && a.COn == null &&a.COff!=null);
                TAbsence ab_off = db.TAbsences.FirstOrDefault(a => a.CApplyNumber == applyNum && a.COff == null&&a.COn!=null);
                //TAbsence ab_none = db.TAbsences.FirstOrDefault(a => a.CApplyNumber == applyNum && a.COff == null && a.COn==null);

                if (ab_on != null)
                {
                    ab_on.COn = DateTime.Parse(ab_on.COff.Value.Year + "-" + ab_on.COff.Value.Month + "-" + ab_on.COff.Value.Day + " " + "09:00:00");
                    ab_on.CStatus = "準時";
                    db.SaveChanges();
                }
                else if (ab_off != null)
                {
                    ab_off.COff = DateTime.Parse( ab_off.COn.Value.Year + "-" + ab_off.COn.Value.Month + "-" + ab_off.COn.Value.Day + " " + "18:00:00") ;
                    DateTime t = DateTime.Parse(ab_off.COn.Value.Year + "-" + ab_off.COn.Value.Month + "-" + ab_off.COn.Value.Day + " " + "09:00:00");
                    if (ab_off.COn.Value<= t)
                        ab_off.CStatus = "準時";
                    else if(ab_off.COn.Value >= t)
                        ab_off.CStatus = "遲到";

                    db.SaveChanges();
                }
                //else if(ab_none != null)
                //{

                //}
            }
            return RedirectToAction("List");
        }
        #endregion
        public JsonResult yesterdayVal(string ysd)//判斷前一天是否有打卡
        {
            
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            string[]str= ysd.Split(",");
            DateTime ye = DateTime.Parse( str[1]);
            TAbsence ab = db.TAbsences.FirstOrDefault(a=>a.CEmployeeId==userId&&
            a.COn.Value.ToString().Contains(ye.ToString())&&
            a.COff.Value.ToString().Contains(ye.ToString()));
            var day = ye.DayOfWeek;

            if (ab==null)
            {
                if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)//判斷是否為六日
                {
                    TAbsence absence = new TAbsence()
                    {
                        CEmployeeId = userId,
                        CStatus = "異常"
                    };
                    db.Add(absence);
                    db.SaveChanges();
                }
            }
            return Json(ysd);
        }
        public string Clock()//打卡
        {
            //var t = from clock in db.TAbsences
            //        let x = DateTime.Today.ToString("yyyy-MM-dd")
            //        where clock.CEmployeeId.ToString() == ViewData[CDictionary.CURRENT_LOGINED_USERID].ToString()
            //        select clock;
            return "";
        }
        public IActionResult date_search(DateTime? sDate,DateTime? eDate,int? opVal)
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = db.TAbsences
                .Where(a => (sDate != null ? a.COn.Value.Date >= sDate : true) &&
                      (sDate != null ? a.COff.Value.Date >= sDate : true) &&
                      (eDate != null ? a.COn.Value.Date <= eDate : true) &&
                      (eDate != null ? a.COff.Value.Date <= eDate : true)&&
                      (opVal!=null?a.COn.Value.Date==))
                .OrderBy(a=>(a.COn!=null?a.COn:a.COff)).ToList();

            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in time)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    COn = item.COn,
                    COff = item.COff,
                    CSatus = item.CStatus
                };
                list.Add(avm);
            }


            return PartialView("date_search",list);
        }

    }
}
