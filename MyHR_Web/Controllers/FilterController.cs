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
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (HttpContext.Session.GetObject<TUser>(CDictionary.Current_User) == null)
                filterContext.Result = RedirectToAction("Login","Login");
        }
    }
}
