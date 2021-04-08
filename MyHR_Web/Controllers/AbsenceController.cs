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
        DateTime Late = DateTime.Today.AddHours(10); //9:59(含)前都算遲到，之後都顯示異常，須請假

        DateTime now = DateTime.Now;
        #region 上下班打卡
        public IActionResult List()
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = from t in db.TAbsences.AsEnumerable()
                       where t.CEmployeeId == userId
                       orderby t.CDate descending
                       select t;

            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in time)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber=item.CApplyNumber,
                    CDate=item.CDate,
                    COn=item.COn,
                    COff=item.COff,
                    CStatus=item.CStatus
                };
                list.Add(avm);
            }
            return View(list);
        }

        public IActionResult getClockString_on(int id,DateTime date)//上班
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            DateTime now = DateTime.Now;

            TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == id && z.COn.HasValue);//尋找該員工今天的打卡紀錄
            if (td == null)//今天未打卡
            {
                if (now <= Con)//9:00前
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = id,
                        CDate= date,
                        COn = TimeSpan.Parse(now.ToString("hh:mm:ss")),
                        CStatus = "準時"
                    };
                    db.TAbsences.Add(b);
                }
                else if (now > Con /*&& now< Late*/)//9:00-
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = id,
                        CDate = date,
                        COn = TimeSpan.Parse(now.ToString("hh:mm:ss")),
                        CStatus = "遲到"
                    };
                    db.TAbsences.Add(b);
                }
                //else if (now >= Late)//10:00之後
                //{
                //    TAbsence b = new TAbsence()
                //    {
                //        CEmployeeId = int.Parse(id),
                //        CDate = DateTime.Parse(date),
                //        COn = TimeSpan.Parse(time),
                //        CStatus = "異常"
                //    };
                //    db.TAbsences.Add(b);
                //}
                db.SaveChanges();
            }
            var table = db.TAbsences
                           .Where(a => a.CEmployeeId == userId)
                           .OrderByDescending(a => a.CDate).ToList();
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in table)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CDate = item.CDate,
                    COn = item.COn,
                    COff = item.COff,
                    CStatus = item.CStatus
                };
                list.Add(avm);
            }

            return PartialView("date_search",list);
        }
        public IActionResult getClockString_off(int id, DateTime date)//下班
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            DateTime now = DateTime.Now;

            TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == id && z.COn.HasValue);//尋找該員工今天的打卡紀錄

            if (td != null)//有打上班卡
            {
                TimeSpan aonTime = td.COn.Value;//上班卡的時間
                TimeSpan ConTime = Con.TimeOfDay;//09:00
                if (date == td.CDate)
                {
                    if (aonTime > ConTime)
                    {
                        td.COff = TimeSpan.Parse(now.ToString("hh:mm:ss"));
                        td.CStatus = "遲到";
                        db.SaveChanges();
                    }
                    else if (aonTime < ConTime)
                    {
                        td.COff = TimeSpan.Parse(now.ToString("hh:mm:ss"));
                        td.CStatus = "準時";
                        db.SaveChanges();
                    }
                }
            }
            else if (td == null)//沒打上班卡
            {
                TAbsence b = new TAbsence()
                {
                    CEmployeeId = id,
                    CDate = date,
                    COff = TimeSpan.Parse(now.ToString("hh:mm:ss")),
                    CStatus ="異常"
                };
                db.TAbsences.Add(b);
                db.SaveChanges();
            }
            var table = db.TAbsences
                       .Where(a => a.CEmployeeId == userId)
                       .OrderByDescending(a => a.CDate).ToList();
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in table)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CDate = item.CDate,
                    COn = item.COn,
                    COff = item.COff,
                    CStatus = item.CStatus
                };
                list.Add(avm);
            }
            return PartialView("date_search", list);
        }
        #endregion

        #region Edit打卡補登

        public IActionResult Edit(int? applyNum)
        {
            if (applyNum!=null)
            {
                ViewBag.absence = applyNum;
                TAbsence abs = db.TAbsences.FirstOrDefault(a=>a.CApplyNumber==applyNum);
                if (abs!=null)
                {
                    CAbsenceViewModel obj = new CAbsenceViewModel()
                    {
                        CApplyNumber = abs.CApplyNumber,
                        CDate = abs.CDate,
                        CEmployeeId = abs.CEmployeeId,
                        COn = abs.COn,
                        COff = abs.COff
                    };
                    return View(obj);
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(TAbsence absence,int? id, DateTime? date, string? when,int? applyNum)
        {
            if (absence != null)
            {
                TAbsence absed = db.TAbsences.FirstOrDefault(a=>a.CApplyNumber== applyNum);
                TimeSpan ConTime = Con.TimeOfDay;//09:00
                TimeSpan tenOclck = Con.AddHours(1).TimeOfDay;//09:00

                if (absed != null)
                {
                    if (absed.COn == null&& absed.COff!=null )//補上班卡
                    {
                        absed.COn = TimeSpan.Parse("09:00:00");
                        absed.CStatus = "準時";
                    }
                    else if (absed.COn != null && absed.COff==null)//補下班卡
                    {
                        absed.COff = TimeSpan.Parse("18:00:00"); ;
                        if (absed.COn > ConTime)//9:01
                        {
                            absed.CStatus = "遲到";
                        }
                        else if (absed.COn <= ConTime)//9:00前
                        {
                            absed.CStatus = "準時";
                        }
                    }
                    else if (absed.COn == null && absed.COff == null)//補上下班卡
                    {
                        if (when=="上班")
                        {
                            absed.COn = TimeSpan.Parse("09:00:00");
                            absed.CStatus = "異常";
                        }
                        else if (when == "下班")
                        {
                            absed.COff = TimeSpan.Parse("18:00:00");
                            absed.CStatus = "異常";
                        }
                    }
                    db.Update(absed);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        #endregion
        public IActionResult yesterdayVal(DateTime ysd)//判斷前一天是否有打卡
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            TAbsence ab = db.TAbsences.FirstOrDefault(a => a.CEmployeeId == userId && a.CDate.Value.Date== ysd);//尋找昨天的打卡紀錄
            var day = ysd.DayOfWeek;//星期

            if (ab == null)
            {
                if (day != DayOfWeek.Sunday || day != DayOfWeek.Saturday)//判斷昨天是否為六日
                {
                    TAbsence absence = new TAbsence()
                    {
                        CEmployeeId = userId,
                        CDate = ysd,
                        CStatus = "異常"
                    };
                    db.Add(absence);
                    db.SaveChanges();
                }
            }
            var table = db.TAbsences
                       .Where(a => a.CEmployeeId == userId)
                       .OrderByDescending(a => a.CDate).ToList();
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in table)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CDate = item.CDate,
                    COn = item.COn,
                    COff = item.COff,
                    CStatus = item.CStatus
                };
                list.Add(avm);
            }
            return PartialView("date_search", list);
        }
        public IActionResult date_search(DateTime? sDate, DateTime? eDate)
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = db.TAbsences
                .Where(a => a.CEmployeeId==userId&&
                      (sDate != null ? a.CDate >= sDate : true)&&
                      (eDate != null ? a.CDate <= eDate : true))
                .OrderByDescending(a=>a.CDate).ToList();

            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (var item in time)
            {
                CAbsenceViewModel avm = new CAbsenceViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CDate=item.CDate,
                    COn = item.COn,
                    COff = item.COff,
                    CStatus = item.CStatus
                };
                list.Add(avm);
            }
            return PartialView("date_search",list);
        }

    }
}
