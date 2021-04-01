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
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
            //{
            //    Random rm = new Random();
            //    string code = rm.Next(0, 10).ToString() + rm.Next(0, 10).ToString()
            //        + rm.Next(0, 10).ToString() + rm.Next(0, 10).ToString();

            //    HttpContext.Session.SetString(CDictionary.LOGIN_AUTHTICATION_CODE, code);
            //}


            return PartialView();

        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel p)
        {
            //if (!p.txtCode.Equals(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
            //{
            //    ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
            //    return View();
            //}

            //ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);


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
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, user.CEmployeeId.ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERENNAME, user.CEmployeeEnglishName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PASSWORD, user.CPassWord);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_OBD, (user.COnBoardDay).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BBD, (user.CByeByeDay).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_GENDER, user.CGender);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMAIL, user.CEmail);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_ADDRESS, user.CAddress);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_SUPERVISOR, (user.CSupervisor).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BRD, (user.CBirthday).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PHONE, user.CPhone);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_PER, user.CEmergencyPerson);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_CONT, user.CEmergencyContact);
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


        [HttpPost]
        public IActionResult AccEnable(CLoginViewModel p, TUserViewModel _user)
        {
            if (p.txtAccount != null && p.txtPassword != null)
            {

                if (_user != null)
                {

                    TUser u = db.TUsers.FirstOrDefault(u => u.CEmployeeId == int.Parse(p.txtAccount) && u.CPassWord == p.txtPassword);
                    if (u != null)
                    {
                        u.CAccountEnable = 1;
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "錯誤的帳號或密碼";
                    }
                }
            }
            return RedirectToAction("Login");


        }

    }
}
