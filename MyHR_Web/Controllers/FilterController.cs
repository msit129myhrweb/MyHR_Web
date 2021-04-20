using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyHR_Web.Models;
using MyHR_Web.MyClass;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class FilterController : Controller
    {
        dbMyCompanyContext myHR = new dbMyCompanyContext();
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (HttpContext.Session.GetObject<TUser>(CDictionary.Current_User) == null)
                filterContext.Result = RedirectToAction("Login","Login");
        }
        
        // 新增通知
        public void AddNoti(int toUserId, string notiHeader, string notiBody)
        {            
            var user = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User);
            TNotification noti = new TNotification();
            noti.FromUserId = user.CEmployeeId;
            noti.ToUserId = toUserId;
            noti.NotiHeader = notiHeader;
            noti.NotiBody = notiBody;
            noti.IsRead = false;
            noti.Url = "123";
            noti.CreatedDate = DateTime.Now;

            myHR.Add(noti);
            myHR.SaveChanges();
        }
    }
}
