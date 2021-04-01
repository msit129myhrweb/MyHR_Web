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

using System.IO;


namespace MyHR_Web.Controllers
{  //現在版本!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private dbMyCompanyContext db = new dbMyCompanyContext();

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

        [HttpGet]
        public IActionResult ViewPhoto(int id)
        {
            var photo = db.TUsers.FirstOrDefault(u => u.CEmployeeId == id);
            MemoryStream ms = new MemoryStream(photo.CPhoto);
            return new FileStreamResult(ms, "image/JPG");
        }


        public IActionResult Profile()
        {            
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERJOBTITLE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE);
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData["USERID"] = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            int userid = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            var table = from u in db.TUsers
                        where u.CEmployeeId == userid
                        select u;
            //return View(table);

            List<TUserViewModel> list = new List<TUserViewModel>();
            foreach (TUser i in table)
                list.Add(new TUserViewModel(i));
            return View(list);
        }


        public IActionResult ProfileEdit(int? id)
        {
            ViewData["USERID"] = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERJOBTITLE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE);



            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENTID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID);

            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_USERENNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERENNAME);
            ViewData[CDictionary.CURRENT_LOGINED_PASSWORD] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_PASSWORD);
            ViewData[CDictionary.CURRENT_LOGINED_OBD] = (HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_OBD));
            ViewData[CDictionary.CURRENT_LOGINED_BBD] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_BBD);
            ViewData[CDictionary.CURRENT_LOGINED_GENDER] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_GENDER);
            ViewData[CDictionary.CURRENT_LOGINED_EMAIL] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_EMAIL);
            ViewData[CDictionary.CURRENT_LOGINED_ADDRESS] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_ADDRESS);
            ViewData[CDictionary.CURRENT_LOGINED_SUPERVISOR] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_SUPERVISOR);
            ViewData[CDictionary.CURRENT_LOGINED_BRD] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_BRD);
            ViewData[CDictionary.CURRENT_LOGINED_PHONE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_PHONE);
            ViewData[CDictionary.CURRENT_LOGINED_EMERGENCY_PER] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_EMERGENCY_PER);
            ViewData[CDictionary.CURRENT_LOGINED_EMERGENCY_CONT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_EMERGENCY_CONT);
            ViewData[CDictionary.CURRENT_LOGINED_OB_STATUS] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_OB_STATUS);
            ViewData[CDictionary.CURRENT_LOGINED_ACC_ENABLE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_ACC_ENABLE);
            if (id != null)
            {

                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == id);

                if (u != null)
                {
                    return View(new TUserViewModel(u));

                }

            }
            return RedirectToAction("Profile");
        }


        [HttpPost]
        public async Task<IActionResult> ProfileEdit(TUser user, TUserViewModel user_vm, List<IFormFile> CPhoto)
        {


            user = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User);//取一個在session中的TUser物件(可抓到id)

            foreach (var item in CPhoto)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        user.CPhoto = stream.ToArray();
                    }
                }

                if (user != null)
                {
                    db.Update(user);
                    db.SaveChanges();
                }
            }
            //return View();
            if (user_vm != null)
            {

                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == user_vm.CEmployeeId);
                if (u != null)
                {
                    u.CEmployeeEnglishName = user_vm.CEmployeeEnglishName;
                    u.CPassWord = user_vm.CPassWord;
                    u.CGender = user_vm.CGender;
                    u.CEmail = user_vm.CEmail;
                    u.CAddress = user_vm.CAddress;
                    u.CBirthday = (DateTime)user_vm.CBirthday;
                    u.CPhone = user_vm.CPhone;
                    u.CEmergencyPerson = user_vm.CEmergencyPerson;
                    u.CEmergencyContact = user_vm.CEmergencyContact;
                    u.CAccountEnable = user_vm.CAccountEnable;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Profile");
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
