using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Helpers;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    
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
            int userid = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            var table = from u in (new dbMyCompanyContext()).TUsers
                        where u.CEmployeeId == userid
                        select u;
            //return View(table);




            List<TUserViewModel> list = new List<TUserViewModel>();
            foreach (TUser i in table)
                list.Add(new TUserViewModel(i));
            return View(list);
            //return View(table);
        }
        public IActionResult ProfileEdit(int? id)
        {
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
                dbMyCompanyContext db = new dbMyCompanyContext(); 
                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == id);

                if (u != null)
                {
                    return View(new TUserViewModel(u));
                }

            }
            return RedirectToAction("Profile");
        }


        [HttpPost]
        public IActionResult ProfileEdit(TUserViewModel Tuser_vm)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            if (Tuser_vm != null)
            {

                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == Tuser_vm.CEmployeeId);
                if (u != null)
                { u.CEmployeeEnglishName = Tuser_vm.CEmployeeEnglishName;
                    u.CPassWord =Tuser_vm.CPassWord; 
                    u.CGender = Tuser_vm.CGender;   
                    u.CEmail = Tuser_vm.CEmail;
                    u.CAddress = Tuser_vm.CAddress;
                    u.CBirthday = (DateTime)Tuser_vm.CBirthday;
                    u.CPhone = Tuser_vm.CPhone;
                    u.CEmergencyPerson = Tuser_vm.CEmergencyPerson;
                    u.CEmergencyContact = Tuser_vm.CEmergencyContact;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Profile");
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
            if(p.txtAccount!=null ||p.txtPassword!=null)
            { 
                TUser user = (new dbMyCompanyContext()).TUsers.FirstOrDefault(c =>
                c.CEmployeeId.Equals(Int32.Parse(p.txtAccount)) && c.CPassWord.Equals(p.txtPassword));
            
                if (user != null)
                {
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, user.CEmployeeName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID, (user.CDepartmentId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT, ((eDepartment)user.CDepartmentId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE, ((eJobTitle)user.CJobTitleId).ToString());

                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID,(user.CEmployeeId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERENNAME,user.CEmployeeEnglishName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PASSWORD , user.CPassWord);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_OBD, (user.COnBoardDay).ToString("yyyy/MM/dd"));
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BBD, (user.CByeByeDay).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_GENDER, user.CGender);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMAIL, user.CEmail);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_ADDRESS, user.CAddress);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_SUPERVISOR, (user.CSupervisor).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_BRD, (user.CBirthday).ToString("yyyy/MM/dd"));
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_PHONE, user.CPhone);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_PER, user.CEmergencyPerson);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_EMERGENCY_CONT, user.CEmergencyContact);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_OB_STATUS, ((eOnBoard)user.COnBoardStatusId).ToString());
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_ACC_ENABLE, ((eAccount)user.CAccountEnable).ToString());

                    return RedirectToAction("Index");
                }
            }
            

            return PartialView();
        }

       

        [HttpPost]
        public IActionResult register(TUserViewModel _user)
        {

            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TUsers.Add(_user.tuserVM);
            db.SaveChanges();
            return RedirectToAction("Login");


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
     
        //todo wz
        public JsonResult UploadOneFile(IFormFile photo)
        {
            //if (photo.Length > 0)
            //{
            //    using (var photo1 = new FileStream(
            //    iv_host.WebRootPath + @"\Upload\" + photo.FileName,
            //   FileMode.Create))
            //    {
            //        photo.CopyTo(photo1);
            //    }

            //    return Json(new { result = true, msg = "上傳成功" });
            //}
            return Json(new { result = false, msg = "上傳失敗" });
        }
        //todo wz



       

    }
}
