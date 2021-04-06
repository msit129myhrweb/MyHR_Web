using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Exts;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{

    public class BaseController : FilterController
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        protected int getUserDepartmentId()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID).TryToInt().GetValueOrDefault();
        }
        /// <summary>
        /// 取得UserID
        /// </summary>
        /// <returns></returns>
        protected int getUserId()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_USERID).TryToInt().GetValueOrDefault();
        }
        private string GetSessionString(string key)
        {
            return HttpContext.Session.GetString(key);
        }
        protected string getUserPhone()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_PHONE);
        }
        protected string getUserName()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_USERNAME);
        }

    }
}
