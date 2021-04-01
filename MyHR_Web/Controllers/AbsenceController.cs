﻿using System;
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
        #region 上下班打卡
        public IActionResult List()
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            #region 判斷打卡狀態

            DateTime off = DateTime.Today.AddHours(18); //下班時間18:00
            DateTime on = DateTime.Today.AddHours(9); //上班時間9:00

            var time = from t in db.TAbsences.AsEnumerable()
                       where t.CEmployeeId == userId
                       select new
                       {
                           t.CApplyNumber,
                           t.COn,
                           t.COff
                       };
            List<CAbsenceViewModel> alist = new List<CAbsenceViewModel>();
            foreach (var item in time)
            {
                CAbsenceViewModel obj = new CAbsenceViewModel()
                {
                    CApplyNumber=item.CApplyNumber,
                    COn = item.COn,
                    COff = item.COff
                };
                alist.Add(obj);
            }
            List<CAbsenceViewModel> aL = new List<CAbsenceViewModel>();
            foreach (var item in alist)
            {
                int num = item.CApplyNumber;
                DateTime? a = item.COn;
                DateTime? b = item.COff;
                CAbsenceViewModel aVM = new CAbsenceViewModel();
                if (a == null ||b == null)
                {
                    aVM.CApplyNumber = num;
                    aVM.COn = a;
                    aVM.COff = b;
                    aVM.status = "異常"; 
                }
                else
                {
                    a = item.COn.Value;
                    b = item.COff.Value;

                    if (a < on && b > off)
                    {
                        aVM.CApplyNumber = num;
                        aVM.COn = a;
                        aVM.COff = b;
                        aVM.status = "準時";
                    }
                    else if (a > on && b > off)
                    {
                        aVM.CApplyNumber = num;
                        aVM.COn = a;
                        aVM.COff = b;
                        aVM.status = "遲到";
                    }
                    else if (a < on && b < off)
                    {
                        aVM.CApplyNumber = num;
                        aVM.COn = a;
                        aVM.COff = b;
                        aVM.status = "早退";
                    }
                    else if (a > on && b < off)
                    {
                        aVM.CApplyNumber = num;
                        aVM.COn = a;
                        aVM.COff = b;
                        aVM.status = "遲到早退";
                    }
                }
                aL.Add(aVM);
            }
            #endregion

            var table = from absence in db.TAbsences.AsEnumerable()
                        join a_vm in aL on absence.CApplyNumber equals a_vm.CApplyNumber
                        where absence.CEmployeeId== userId
                        select new {
                            absence.COn,
                            absence.COff,
                            a_vm.status
                        };
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();

            foreach (var item in table)
            {
                CAbsenceViewModel newObj = new CAbsenceViewModel()
                {
                    COn=item.COn,
                    COff=item.COff,
                    status=item.status
                };
                    list.Add(newObj);
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





        public string Clock()//打卡
        {
            var t = from clock in db.TAbsences
                    let x = DateTime.Today.ToString("yyyy-MM-dd")
                    where clock.CEmployeeId.ToString() == ViewData[CDictionary.CURRENT_LOGINED_USERID].ToString()
                    select clock;
            return "";

        }
        public string showClockStatus()
        {

            return "";
        }
    }
}
