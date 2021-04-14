using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyHR_Web.Models;
using MyHR_Web.MyClass;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class CalendarController : Controller
    {


        public IActionResult Calendar()
        {
            int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            ViewData["userId"] = userId;
            return View();
        }


     
        public JsonResult GetEvents()
        {
            using (dbMyCompanyContext db = new dbMyCompanyContext())
            {
                int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                
                var events = db.TEvents.Where(c=>c.EmployeeId== userId).Select(n => new
                {
                    EventId = n.EventId,
                    EmployeeId = userId,
                    Subject = n.Subject,
                    Start = n.Start,
                    Description = n.Description,
                    End = n.End,
                    IsFullDay = n.IsFullDay,
                    ThemeColor = n.ThemeColor,

                }).ToList();

                //var leave = db.TLeaveApplications.Select(n => new
                //{
                //    EventId = 0,
                //    EmployeeId=userId,
                //    Subject = ((eLeaveCategory)n.CLeaveCategory).ToString(),
                //    Start = DateTime.Parse(n.CLeaveStartTime),
                //    Description = n.CReason,
                //    End = DateTime.Parse(n.CLeaveEndTime),
                //    IsFullDay = false,
                //    ThemeColor = "red",

                //});

                var travel = db.TTravelExpenseApplications.Select(n => new
                {
                    EventId = 0,
                    EmployeeId = userId,
                    Subject = n.CReason,
                    Start = n.CTravelStartTime,
                    Description = n.CReason,
                    End = n.CTravelEndTime,
                    IsFullDay = false,
                    ThemeColor = "red",

                }).ToList();

                var Data =
                    (
                    from n in events
                    select new
                    {
                        EventId = n.EventId,
                        EmployeeId = n.EmployeeId,
                        Subject = n.Subject,
                        Start = n.Start,
                        Description = n.Description,
                        End = n.End,
                        IsFullDay = (bool)n.IsFullDay,
                        ThemeColor = n.ThemeColor,
                    }

                )
                .Concat
                (
                    from l in travel
                    select new
                    {
                        EventId = l.EventId,
                        EmployeeId = l.EventId,
                        Subject = l.Subject,
                        Start = l.Start,
                        Description = l.Description,
                        End = l.End,
                        IsFullDay = l.IsFullDay,
                        ThemeColor = l.ThemeColor,
                    }

                );

               // var Data =
               //    (
               //    from n in events
               //    select n
               //)
               //.Concat
               //(
               //    from l in travel
               //    select l
               //);



                return new JsonResult(Data);
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(CalendarViewModel e)
        {
            var status = false;
            using (dbMyCompanyContext db = new dbMyCompanyContext())
            {
                int userId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
                ViewData["userId"] = userId;
                if (e.EventId > 0)
                {
                    //Update the event
                    var v = db.TEvents.Where(a => a.EventId == e.EventId).FirstOrDefault();
                    if (v != null)
                    {
                        v.EmployeeId = userId;
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    db.TEvents.Add(e.tevents);
                }

                db.SaveChanges();
                status = true;

            }
            var Data = status;
            //return new JsonResult { Data = new { status = status } };
            return new JsonResult(Data);

        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (dbMyCompanyContext db = new dbMyCompanyContext())
            {
                var v = db.TEvents.Where(a => a.EventId == eventID).FirstOrDefault();
                if (v != null)
                {
                    db.TEvents.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            var Data = status;
            //return new JsonResult { Data = new { status = status } };
            return new JsonResult(Data);

        }
    }
}
