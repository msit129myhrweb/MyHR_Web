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
                       orderby t.CDate
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

        public JsonResult getClockString_on(string c)//上班
        {
            string[] ids = c.Split('\"', '"', ',',' ');
            var id=ids[1];
            var date = ids[2];
            var time = ids[4];

            TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id)&& z.COn.HasValue);//尋找該員工今天的打卡紀錄
            //TAbsence td_on = db.TAbsences.FirstOrDefault(z => z.COff == TimeSpan.Zero && z.CEmployeeId == int.Parse(id));
            if (td == null)//今天未打卡
            {
                if (now <= Con)//9:00前
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = int.Parse(id),
                        CDate=DateTime.Parse(date),
                        COn = TimeSpan.Parse(time),
                        CStatus = "準時"
                    };
                    db.TAbsences.Add(b);
                }
                else if (now > Con && now< Late)//9:00-9:59
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = int.Parse(id),
                        CDate = DateTime.Parse(date),
                        COn = TimeSpan.Parse(time),
                        CStatus = "遲到"
                    };
                    db.TAbsences.Add(b);
                }
                else if (now >= Late)//10:00之後
                {
                    TAbsence b = new TAbsence()
                    {
                        CEmployeeId = int.Parse(id),
                        CDate = DateTime.Parse(date),
                        COn = TimeSpan.Parse(time),
                        CStatus = "異常"
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

            TAbsence td = db.TAbsences.FirstOrDefault(z => z.CDate.Value.Date == DateTime.Today && z.CEmployeeId == int.Parse(id) && z.COn.HasValue);//尋找該員工今天的打卡紀錄

            if (td != null)//有打上班卡
            {
                string y = td.CDate.Value.Year.ToString();
                string m = td.CDate.Value.Month.ToString();
                string d = td.CDate.Value.Day.ToString();
                TimeSpan aonTime = td.COn.Value;//上班卡的時間
                TimeSpan ConTime = Con.TimeOfDay;//09:00

                if (m.Length < 2 && d.Length <2)//1-9月，1-9日
                {
                     m = "0" + td.CDate.Value.Month.ToString();
                     y = td.CDate.Value.Year.ToString();
                     d = "0" + td.CDate.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length < 2 && d.Length > 2)//1-9月，10-31日
                {
                    m = "0" + td.CDate.Value.Month.ToString();
                    y = td.CDate.Value.Year.ToString();
                    d = td.CDate.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length > 2 && d.Length < 2)//10-12月，1-9日
                {
                    m = td.CDate.Value.Month.ToString();
                    y = td.CDate.Value.Year.ToString();
                    d = "0" + td.CDate.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        { 
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                    }
                }
                else if (m.Length > 2 && d.Length > 2)//10-12月，10-31日
                {
                    m = td.CDate.Value.Month.ToString();
                    y = td.CDate.Value.Year.ToString();
                    d = td.CDate.Value.Day.ToString();
                    string ON = y + "-" + m + "-" + d;
                    if (date == ON)
                    {
                        if (aonTime > ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                        else if (aonTime < ConTime)
                        {
                            td.COff = TimeSpan.Parse(time);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else if (td == null)//沒打上班卡
            {
                TAbsence b = new TAbsence()
                {
                    CEmployeeId = int.Parse(id),
                    CDate = DateTime.Parse(date),
                    COff = TimeSpan.Parse(time),
                    CStatus ="異常"
                };
                db.TAbsences.Add(b);
                db.SaveChanges();
            }
            return Json(c);
        }
        #endregion

        #region Edit打卡補登

        public IActionResult Edit(int? applyNum)
        {
            if (applyNum!=null)
            {
                TAbsence abs = db.TAbsences.FirstOrDefault(a=>a.CApplyNumber==applyNum);
                //TUser user = db.TUsers.FirstOrDefault(u=>u.CEmployeeId==absence.CEmployeeId) ;
                //TUserDepartment dep = db.TUserDepartments.FirstOrDefault(d=>d.CDepartmentId==user.CDepartmentId);
                if (abs!=null)
                {
                    CAbsenceViewModel obj = new CAbsenceViewModel()
                    {
                        CApplyNumber = abs.CApplyNumber,
                        CDate = abs.CDate,
                        CEmployeeId = abs.CEmployeeId,
                        COn = abs.COn,
                        COff = abs.COff
                        //CDepartment=dep.CDepartment,
                        //employeeName=user.CEmployeeName
                    };
                    return View(obj);
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(TAbsence absence)
        {
            if (absence != null)
            {
                TAbsence absed = db.TAbsences.FirstOrDefault(a=>a.CApplyNumber==absence.CApplyNumber);
                if (absed!=null)
                {
                    if (absence.COn!=null)
                    {
                    absed.COff = absence.COff;

                    absed.CStatus = "";

                    }
                }
            }
            return RedirectToAction("List");
        }
        #endregion
        public JsonResult yesterdayVal(string ysd)//判斷前一天是否有打卡
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            string[] str = ysd.Split(",");
            DateTime ye = DateTime.Parse(str[1]);
            TAbsence ab = db.TAbsences.FirstOrDefault(a => a.CEmployeeId == userId && a.CDate.Value.ToString().Contains(ye.ToString()));//尋找昨天的打卡紀錄
            var day = ye.DayOfWeek;//星期

            if (ab == null && day != DayOfWeek.Sunday || day != DayOfWeek.Saturday)//判斷昨天是否為六日
            {
                TAbsence absence = new TAbsence()
                {
                    CEmployeeId = userId,
                    CDate = ye,
                    CStatus = "異常"
                };
                db.Add(absence);
                db.SaveChanges();
            }
            return Json(ysd);
        }
        public IActionResult date_search(DateTime? sDate, DateTime? eDate)
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var time = db.TAbsences
                .Where(a => a.CEmployeeId==userId&&
                      (sDate != null ? a.CDate >= sDate : true)&&
                      (eDate != null ? a.CDate <= eDate : true))
                .OrderBy(a=>a.CDate).ToList();

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
