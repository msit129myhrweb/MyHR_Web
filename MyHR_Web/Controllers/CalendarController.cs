using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyHR_Web.Models;
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
            return View();
        }

        public JsonResult GetEvents()
        {
            using (dbMyCompanyContext db = new dbMyCompanyContext())
            {
                var events = db.TEvents.Select(n => new
                {
                    EventId = n.EventId,
                    Subject = n.Subject,
                    Start = n.Start,
                    Description = n.Description,
                    End = n.End,
                    IsFullDay = n.IsFullDay,
                    ThemeColor = n.ThemeColor,

                }).ToList();
                //return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                var Data = events;
                return new JsonResult(Data);
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(TEvent e)
        {
            var status = false;
            using (dbMyCompanyContext db = new dbMyCompanyContext())
            {
                if (e.EventId > 0)
                {
                    //Update the event
                    var v = db.TEvents.Where(a => a.EventId == e.EventId).FirstOrDefault();
                    if (v != null)
                    {
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
                    db.TEvents.Add(e);
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
