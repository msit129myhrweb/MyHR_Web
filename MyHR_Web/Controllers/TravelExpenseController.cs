using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Models
{
    public class TravelExpenseController : Controller
    {
        public IActionResult List()
        {
            //string keyword = Request.Form["txtQuery"];
            //List<TTravelExpenseApplication> list = null;
            //if (string.IsNullOrEmpty(keyword))
            //{
            //    list = (new CTravelFactory()).getAll();
            //}
            //else
            //{
            //    list = (new CTravelFactory()).getByKeyword(keyword);
            //}
            var travaletable = from r in (new dbMyCompanyContext()).TTravelExpenseApplications
                               select r;
            return View(travaletable);
            //return View(list);
            
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TTravelExpenseApplication t)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TTravelExpenseApplications.Add(t);
            db.SaveChanges();
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
