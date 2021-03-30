using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class employeeController : Controller
    {
       
        public IActionResult register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult register(TUserViewModel _user)
        {

            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TUsers.Add(_user.tuserVM);
            db.SaveChanges();
            return RedirectToAction("employeeList");


        }

        public IActionResult employeeList()
        {

            dbMyCompanyContext db = new dbMyCompanyContext();
             var table = from r in db.TUsers
                    select r;
            List<TUserViewModel> list = new List<TUserViewModel>();
            foreach (TUser i in table)
                list.Add(new TUserViewModel(i));
            return View(list);


        }


        public IActionResult employeeEdit(int? id)
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == id);

                if (u != null)
                {
                    return View(new TUserViewModel(u));
                }

            }
            return RedirectToAction("employeeList");
        }


        [HttpPost]
        public IActionResult employeeEdit(TUserViewModel Tuser_vm)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            if (Tuser_vm != null)
            {

                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == Tuser_vm.CEmployeeId);
                if (u != null)
                {
                    //u.CEmployeeEnglishName = Tuser_vm.CEmployeeEnglishName;
                    //u.CPassWord = Tuser_vm.CPassWord;
                    //u.CGender = Tuser_vm.CGender;
                    //u.CEmail = Tuser_vm.CEmail;
                    //u.CAddress = Tuser_vm.CAddress;
                    //u.CBirthday = (DateTime)Tuser_vm.CBirthday;
                    //u.CPhone = Tuser_vm.CPhone;
                    //u.CEmergencyPerson = Tuser_vm.CEmergencyPerson;
                    //u.CEmergencyContact = Tuser_vm.CEmergencyContact;
                    u.CAccountEnable = Tuser_vm.CAccountEnable;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("employeeList");
        }


    }
}
