using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMyHR_Web.Controllers
{
    public class LostController : Controller
    {
        //todo Reina
        //public IActionResult List()
        //{
        //    var losttable = from l in (new dbMyCompanyContext()).TLosts
        //                   select l;
        //    return View(losttable);
        //}
        public IActionResult Create()
        {
            return View();
        }
        //todo Reina
        //[HttpPost]
        //public IActionResult Create(TLost t)
        //{
        //    dbMyCompanyContext db = new dbMyCompanyContext();
        //    db.TLosts.Add(t);
        //    db.SaveChanges();
        //    return View();
        //}
    }
}
