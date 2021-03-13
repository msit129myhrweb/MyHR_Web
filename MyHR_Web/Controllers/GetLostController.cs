using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class GetLostController : Controller
    {
        //todo Reina
        //public IActionResult GetThing()
        //{
        //    var gettable = from l in (new dbMyCompanyContext()).TLosts
        //                   //from f in (new dbMyCompanyContext()).TFounds
        //                   select l ;
        //    return View(gettable);
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
