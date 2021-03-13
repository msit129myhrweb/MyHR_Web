using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class FoundController : Controller
    {
        //todo Reina
        //public IActionResult List()
        //{
        //   var foundtable = from f in (new dbMyCompanyContext()).TFounds
        //                     select f;
        //    return View(foundtable);
        //}
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TFound f)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            //todo Reina
            //db.TFounds.Add(f);
            db.SaveChanges();
            return View();
        }
    }
}
