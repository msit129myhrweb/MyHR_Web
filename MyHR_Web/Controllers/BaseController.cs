using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Exts;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{

    public class BaseController : Controller
    {
        protected int GetUserDepartmentId()
        {
            return GetSessionString(CDictionary.CURRENT_LOGINED_UserEpartmentId).TryToInt().GetValueOrDefault();
        }
        /// <summary>
        /// 取得UserID
        /// </summary>
        /// <returns></returns>
        protected int GetUserId()
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
        
       
    }
}
