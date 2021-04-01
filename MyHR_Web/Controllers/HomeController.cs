using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHR_Web.Models;
using MyHR_Web.MyClass;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{  //現在版本!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            return View();
        }
        public IActionResult BulletInList()
        {
            return View();
        }
        public IActionResult Profile()
        {            
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERJOBTITLE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE);
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            return View();
        }
        public IActionResult Login()
        {
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
                    HttpContext.Session.SetString(CDictionary.LOGIN_USERPHONE, user.CPhone);

                    return RedirectToAction("Index");
                }
            }

            return PartialView();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Home");
        }

        public IActionResult Calendar()
        {
            ViewData["events"] = new[]
            {
                new CalendarEvent{ Id=1, Title="hang out with Jack", StartDate="2021-03-07"},
                new CalendarEvent{ Id=2, Title="zoom meeting(TSMC project)", StartDate= "2021-03-25"}
            };
            return View();
        }
        public IActionResult ToDoList()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
