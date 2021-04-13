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
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

                var t1 = db.TAbsences
                    .Where(a => a.CEmployeeId == userId &&
                           a.CCountNum > 0 &&
                           a.CCountNum < 3 &&
                           a.CDate.Value.Month == Con.Month)
                    .ToList();
                List<TAbsence> list1 = new List<TAbsence>();
                foreach (var item in t1)
                {
                    TAbsence avm1 = new TAbsence()
                    {
                        CApplyNumber = item.CApplyNumber,
                        CDate = item.CDate,
                        COn = item.COn,
                        COff = item.COff,
                        CStatus = item.CStatus,
                        CCountNum = item.CCountNum
                    };
                    list1.Add(avm1);
                }
                int total = list1.Sum(x => Convert.ToInt32(x.CCountNum));//本月補登總數
                ViewBag.totalCountNum = total;//傳到view

                //預設為顯示當週打卡紀錄
                DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
                DateTime dtSunday = dtMonday.AddDays(6); //當週週日

                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == userId &&
                              a.CDate >= dtMonday &&
                              a.CDate <= dtSunday)
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
                        CStatus = item.CStatus,
                        CCountNum = item.CCountNum
                    };
                    list.Add(avm);
                }
                return View(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult getClockString_on(int id,DateTime date)//上班
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

                TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == id && z.COn.HasValue);//尋找該員工今天的打卡紀錄
                if (td == null)//今天未打卡
                {
                    if (now <= Con)//9:00前
                    {
                        TAbsence b = new TAbsence()
                        {
                            CEmployeeId = id,
                            CDate = date,
                            COn = TimeSpan.Parse(now.ToString("HH:mm:ss")),
                            CStatus = "正常",
                        };
                        db.TAbsences.Add(b);
                    }
                    else if (now > Con && now < Late)//9:00-9:59
                    {
                        TAbsence b = new TAbsence()
                        {
                            CEmployeeId = id,
                            CDate = date,
                            COn = TimeSpan.Parse(now.ToString("HH:mm:ss")),
                            CStatus = "遲到",
                        };
                        db.TAbsences.Add(b);
                    }
                    else if (now >= Late)//10:00(含)之後
                    {
                        TAbsence b = new TAbsence()
                        {
                            CEmployeeId = id,
                            CDate = date,
                            COn = TimeSpan.Parse(now.ToString("HH:mm:ss")),
                            CStatus = "異常",
                        };
                        db.TAbsences.Add(b);
                    }
                    db.SaveChanges();
                }
                DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
                DateTime dtSunday = dtMonday.AddDays(6); //當週週日

                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == userId &&
                              a.CDate >= dtMonday &&
                              a.CDate <= dtSunday)
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
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult getClockString_off(int id, DateTime date)//下班
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                DateTime now = DateTime.Now;

                TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == id && z.COn.HasValue);//尋找該員工今天的打卡紀錄
                TAbsence yd = db.TAbsences.FirstOrDefault(z => z.CEmployeeId == id && z.CDate.Value.Date == DateTime.Today.AddDays(-1));//尋找該員工今天的打卡紀錄

                if (td != null)//有打上班卡
                {
                    TimeSpan aonTime = td.COn.Value;//上班卡的時間
                    TimeSpan ConTime = Con.TimeOfDay;//09:00
                    TimeSpan LateTime = Late.TimeOfDay;//10:00
                    if (date == td.CDate)
                    {
                        if (aonTime > ConTime && aonTime < LateTime)
                        {
                            td.COff = TimeSpan.Parse(now.ToString("HH:mm:ss"));
                            td.CStatus = "遲到";
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            td.COff = TimeSpan.Parse(now.ToString("HH:mm:ss"));
                            td.CStatus = "正常";
                            db.SaveChanges();
                        }
                        else if (aonTime > LateTime)
                        {
                            td.COff = TimeSpan.Parse(now.ToString("HH:mm:ss"));
                            td.CStatus = "異常";
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
                        COff = TimeSpan.Parse(now.ToString("HH:mm:ss")),
                        CStatus = "異常",
                        CCountNum = yd.CCountNum
                    };
                    db.TAbsences.Add(b);
                    db.SaveChanges();
                }
                DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
                DateTime dtSunday = dtMonday.AddDays(6); //當週週日

                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == userId &&
                              a.CDate >= dtMonday &&
                              a.CDate <= dtSunday)
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Edit打卡補登
        public IActionResult Edit(int? applyNum)
        {
            TimeSpan ConTime = Con.TimeOfDay;//09:00
            TimeSpan LateTime = Late.TimeOfDay;//10:00

            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var t1 = db.TAbsences.Where(a => a.CEmployeeId == userId &&
                                             a.CCountNum > 0 &&
                                             a.CCountNum < 3 &&
                                             a.CDate.Value.Month == Con.Month)
                                  .ToList();
            List<TAbsence> list1 = new List<TAbsence>();
            foreach (var item in t1)
            {
                TAbsence avm1 = new TAbsence()
                {
                    CApplyNumber = item.CApplyNumber,
                    CDate = item.CDate,
                    COn = item.COn,
                    COff = item.COff,
                    CStatus = item.CStatus,
                    CCountNum = item.CCountNum
                };
                list1.Add(avm1);
            }
            int total = list1.Sum(x => Convert.ToInt32(x.CCountNum));//本月補登總數
            ViewBag.totalCountNum = total;

            TAbsence abs = db.TAbsences.FirstOrDefault(a => a.CApplyNumber == applyNum);
            ViewBag.absence = applyNum;

            if (applyNum != null  && abs != null && abs.COn == null ? true : abs.COn < LateTime && total < 3)
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
            else
            {
                return RedirectToAction("LeaveCreate", "Leave");
            }
        }
        [HttpPost]
        public IActionResult Edit(TAbsence absence, int? id, DateTime? date, string? when, int? applyNum)
        {
            if (absence != null)
            {
                TAbsence absed = db.TAbsences.FirstOrDefault(a => a.CApplyNumber == applyNum);
                TimeSpan ConTime = Con.TimeOfDay;//09:00
                TimeSpan tenOclck = Con.AddHours(1).TimeOfDay;//10:00

                if (absed != null)
                {
                    if (absed.COn == null && absed.COff != null)//補上班卡
                    {
                        absed.COn = TimeSpan.Parse("09:00:00");
                        absed.CStatus = "正常";
                    }
                    else if (absed.COn != null && absed.COff == null)//補下班卡
                    {
                        absed.COff = TimeSpan.Parse("18:00:00"); ;
                        if (absed.COn > ConTime&& absed.COn< tenOclck)//9:01
                        {
                            absed.CStatus = "遲到";
                        }
                        else if (absed.COn <= ConTime)//9:00前
                        {
                            absed.CStatus = "正常";
                        }
                    }
                    else if (absed.COn == null && absed.COff == null)//補上下班卡
                    {
                        if (when == "上班")
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
                    absed.CCountNum++;
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
                if (day != DayOfWeek.Sunday && day != DayOfWeek.Saturday)//判斷昨天是否為六日
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
            else if (ab != null)//昨天有打上班卡，但未打下班卡
            {
                if (ab.COff == null)
                {
                    ab.CStatus = "異常";
                    db.Update(ab);
                    db.SaveChanges();
                }
            }
            DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
            DateTime dtSunday = dtMonday.AddDays(6); //當週週日

            var table = db.TAbsences
                    .Where(a => a.CEmployeeId == userId &&
                          a.CDate >= dtMonday &&
                          a.CDate <= dtSunday)
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
        public IActionResult date_search(DateTime? sDate, DateTime? eDate,string status)//日期及狀態查詢
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = db.TAbsences
                .Where(a => a.CEmployeeId==userId&&
                      (sDate != null ? a.CDate >= sDate : true)&&
                      (eDate != null ? a.CDate <= eDate : true)&&
                      (status!=null?a.CStatus==status:true))
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

        public IActionResult research(int id)//重新查詢
        {
            DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
            DateTime dtSunday = dtMonday.AddDays(6); //當週週日

            var table = db.TAbsences
                    .Where(a => a.CEmployeeId == id &&
                          a.CDate >= dtMonday &&
                          a.CDate <= dtSunday)
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
        public IActionResult search_dwm(int id,string search_dwm)//當日當週當月查詢
        {
            DateTime currendate = new DateTime();
            
            if (search_dwm== "當日")
            {
                currendate = DateTime.Today;
                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == id &&
                              a.CDate== currendate).ToList();

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
            else if (search_dwm == "當週")
            {
                DateTime dtMonday = DateTime.Now.AddDays(1 - Convert.ToInt16(DateTime.Now.DayOfWeek)); //當週週一
                DateTime dtFriday = dtMonday.AddDays(6); //當週週日

                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == id &&
                              a.CDate >= dtMonday&&
                              a.CDate<= dtFriday)
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
            else//當月
            {
                currendate = DateTime.Today;
                var table = db.TAbsences
                        .Where(a => a.CEmployeeId == id &&
                              a.CDate.Value.Month == currendate.Month)
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
        }
    }
}
