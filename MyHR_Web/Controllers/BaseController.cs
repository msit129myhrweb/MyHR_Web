﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Exts;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{

    public class BaseController : Controller
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        protected int getUserDepartmentId()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_UserEpartmentId).TryToInt().GetValueOrDefault();
        }
        /// <summary>
        /// 取得UserID
        /// </summary>
        /// <returns></returns>
        protected int getUserId()
        {
            return GetSessionString(CDictionary.LOGIN_USERID).TryToInt().GetValueOrDefault();
        }
        private string GetSessionString(string key)
        {
            return HttpContext.Session.GetString(key);
        }
        protected string getUserPhone()
        {
            return GetSessionString(CDictionary.LOGIN_USERPHONE);
        }
        protected string getUserName()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_USERNAME);
        }

    }
}
