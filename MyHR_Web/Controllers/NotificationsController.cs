using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.IService;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHR_Web.MyClass;

namespace MyHR_Web.Controllers
{
    public class NotificationsController : Controller
    {
        private dbMyCompanyContext db = new dbMyCompanyContext();
        INotiService _notiService = null;
        List<Noti> _oNtifications = new List<Noti>();

        public NotificationsController(INotiService nofiService)
        {
            _notiService = nofiService;
        }

        public IActionResult AllNotifications()
        {
            return View();
        }

        public JsonResult GetNotifications(bool bIsGetOnlyUnread=false)
        {
            int nToUserId = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User).CEmployeeId;
            
            _oNtifications = new List<Noti>();
            _oNtifications = _notiService.GetNotifications(nToUserId, bIsGetOnlyUnread);
            return Json(_oNtifications);
        }

        public void AddNotification(List<TNotification> data)
        {
            foreach (var item in data)
                db.TNotifications.Add(item);
            db.SaveChanges();
        }
    }
}
