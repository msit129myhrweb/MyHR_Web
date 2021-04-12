using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class LoginController : Controller
    {

        private dbMyCompanyContext db = new dbMyCompanyContext();
        public IActionResult Login()
        {
            if (HttpContext.Session.GetObject<TUser>(CDictionary.Current_User) != null) //session 存在就跳回首頁
                return RedirectToAction("Index", "Home");
          
            return PartialView();

        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel p)
        {

            string Account = Request.Form["txtAccount"].ToString();
            string Psd = Request.Form["txtPassword"].ToString();

            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Psd))
            {
            }
            else
            {
                TUser user = (new dbMyCompanyContext()).TUsers.FirstOrDefault(c =>
                           c.CEmployeeId.Equals(Int32.Parse(p.txtAccount)) && c.CPassWord.Equals(p.txtPassword));

                if (user != null)
                {
                    HttpContext.Session.SetObject<TUser>(CDictionary.Current_User, user);
                    HttpContext.Session.SetString("Today", DateTime.Now.ToString("yyyy/MM/dd"));
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, user.CEmployeeName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT, ((eDepartment)user.CDepartmentId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID, (user.CDepartmentId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE, ((eJobTitle)user.CJobTitleId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERJOBTITLEID, user.CJobTitleId.ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, user.CEmployeeId.ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERENNAME, user.CEmployeeEnglishName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PASSWORD, user.CPassWord);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_OBD, (user.COnBoardDay).ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BBD, (user.CByeByeDay).ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_GENDER, user.CGender);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMAIL, user.CEmail);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_ADDRESS, user.CAddress);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_SUPERVISOR, (user.CSupervisor).ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BRD, (user.CBirthday).ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PHONE, user.CPhone);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_PER, user.CEmergencyPerson);
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_CONT, user.CEmergencyContact);

                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_OB_STATUS, ((eOnBoard)user.COnBoardStatusId).ToString());
                    //HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_ACC_ENABLE, ((eAccount)user.CAccountEnable).ToString());


                    return RedirectToAction("Index", "Home");
                }
            }

            return PartialView();
        }


        public IActionResult AccEnable()
        {
            return View();
        }


        //[HttpPost]
        //public IActionResult AccEnable(CLoginViewModel p, TUserViewModel _user)
        //{
        //    if (p.txtAccount1 != null && p.txtPassword1 != null)
        //    {

        //        if (_user != null)
        //        {

        //            TUser u = db.TUsers.FirstOrDefault(u => u.CEmployeeId == int.Parse(p.txtAccount1) && u.CPassWord == p.txtPassword1);
        //            if (u != null)
        //            {
        //                u.CAccountEnable = 1;
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                ViewBag.Message = "錯誤的帳號或密碼";
        //            }
        //        }
        //    }
        //    return RedirectToAction("Login");


        //}




        [HttpPost]
        public JsonResult checklogin([FromBody] CLoginViewModel p)
        {
            //CLoginViewModel p = new CLoginViewModel();
            string Data = "";
            if (p.txtAccount != null && p.txtPassword != null)
            {
                using (dbMyCompanyContext db = new dbMyCompanyContext())
                {
                    var user = db.TUsers.Where(a => a.CEmployeeId == int.Parse(p.txtAccount)).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.CPassWord != p.txtPassword)
                        {
                            Data = "密碼錯誤";
                        }
                        else if(user.CAccountEnable==0 && user.COnBoardStatusId==1)
                        {
                            Data = "帳號未啟用";
                        }
                        else if ( user.COnBoardStatusId == 2)
                        {
                            Data = "員工已離職";
                        }
                    }
                    else
                    {
                        Data = "無此帳號";
                    }

                }
            }
            return Json(Data);

        }

        [HttpPost]
        public JsonResult check([FromBody] CPasswordTest p)
        {
            //CLoginViewModel p = new CLoginViewModel();
            string Data = "";
            if (p.txtaccount1 != null && p.txtpassword1 != null)
            {
                using (dbMyCompanyContext db = new dbMyCompanyContext())
                {
                    var user = db.TUsers.Where(a => a.CEmployeeId == int.Parse(p.txtaccount1)).FirstOrDefault();

                    if (user != null)
                    {
                        if (user.CPassWord != p.txtpassword1)
                        {
                            Data = "密碼錯誤";
                        }
                        else
                        {
                            user.CAccountEnable = 1;
                            db.SaveChanges();
                            Data = "報到成功，請重新登入";
                        }
                    }
                    else
                    {
                        Data = "帳號錯誤";
                    }

                }
            }
            return  Json(Data);

        }
    }
}
