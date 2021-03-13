using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Views.Home
{
    public class RepairController : Controller
    {

        public IActionResult RepairList()
        {
            var table = from r in (new dbMyCompanyContext()).TRepairs
                        select r;

            List<CReairViewModel> list = new List<CReairViewModel>();
            foreach (TRepair i in table)
                list.Add(new CReairViewModel(i));
            return View(list);
            //return View(table);
        }
        public IActionResult RepairCreate()
        {
            return View();
        }
    }
}
